using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.Azure;
using System.Data.SqlClient;
using UniversityDbCommon.Models;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace UniversityDbWorker
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private CloudQueue imagesQueue;
        private CloudBlobContainer imagesBlobContainer;
        private string connString;
        private SqlConnection connection;

        public override void Run()
        {
            Trace.TraceInformation("UniversityDbWorker is running");
            CloudQueueMessage message = null;

            while (true)
            {
                try
                {
                    message = imagesQueue.GetMessage();
                    if (message != null)
                    {
                        ProcessQueueMessage(message);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
                catch (StorageException e)
                {
                    if (message != null && message.DequeueCount > 5)
                    {
                        this.imagesQueue.DeleteMessage(message);
                        Trace.TraceError("Deleting poison queue item: '{0}'", message.AsString);
                    }
                    Trace.TraceError("Exception in UniversityDbWorker: '{0}'", e.Message);
                    System.Threading.Thread.Sleep(5000);
                }
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.
            try
            {
                connString = CloudConfigurationManager.GetSetting("UniversityDbConnectionString");

                // Open storage account using credentials from .cscfg file.
                var storageAccount = CloudStorageAccount.Parse
                    (RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

                Trace.TraceInformation("Creating images blob container");
                var blobClient = storageAccount.CreateCloudBlobClient();
                imagesBlobContainer = blobClient.GetContainerReference("university-images");
                if (imagesBlobContainer.CreateIfNotExists())
                {
                    // Enable public access on the newly created "images" container.
                    imagesBlobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });
                }

                Trace.TraceInformation("Creating images queue");
                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                imagesQueue = queueClient.GetQueueReference("university-images");
                imagesQueue.CreateIfNotExists();

                Trace.TraceInformation("Storage initialized");
            }
            catch (Exception)
            {
                throw;
            }

            Trace.TraceInformation("UniversityDbWorker has been started");

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("UniversityDbWorker is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            if (connection != null)
            {
                connection.Close();
            }

            base.OnStop();

            Trace.TraceInformation("UniversityDbWorker has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }

        private void ProcessQueueMessage(CloudQueueMessage message)
        {
            connection = null;
            try
            {
                Trace.TraceInformation("Processing queue message {0}", message);

                connection = new SqlConnection(connString);
                connection.Open();

                var studentId = int.Parse(message.AsString);

                string profileImageUrl = GetProfileImage(studentId);

                if (profileImageUrl == null)
                {
                    throw new Exception(String.Format("Student {0} not found, can't create thumbnail", studentId));
                }

                Uri blobUri = new Uri(profileImageUrl);
                string blobName = blobUri.Segments[blobUri.Segments.Length - 1];

                CloudBlockBlob inputBlob = this.imagesBlobContainer.GetBlockBlobReference(blobName);
                string thumbnailName = Path.GetFileNameWithoutExtension(inputBlob.Name) + "thumb.jpg";
                CloudBlockBlob outputBlob = this.imagesBlobContainer.GetBlockBlobReference(thumbnailName);

                using (Stream input = inputBlob.OpenRead())
                using (Stream output = outputBlob.OpenWrite())
                {
                    ConvertImageToThumbnailJPG(input, output);
                    outputBlob.Properties.ContentType = "image/jpeg";
                }
                Trace.TraceInformation("Generated thumbnail in blob {0}", thumbnailName);

                UpdateProfileThumbnail(studentId, outputBlob.Uri.ToString());

                imagesQueue.DeleteMessage(message);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void UpdateProfileThumbnail(int id, string uri)
        {
            string query = "UPDATE Student SET ProfileThumbnailUrl = @uri WHERE ID=@id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@uri", uri);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
        }

        private string GetProfileImage(int id)
        {
            string url = null;

            string query = "SELECT ProfileImageUrl FROM Student WHERE ID = @id";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                url = reader[0].ToString();
            }
            reader.Close();

            return url;
        }

        public void ConvertImageToThumbnailJPG(Stream input, Stream output)
        {
            int thumbnailsize = 80;
            int width;
            int height;
            var originalImage = new Bitmap(input);

            if (originalImage.Width > originalImage.Height)
            {
                width = thumbnailsize;
                height = thumbnailsize * originalImage.Height / originalImage.Width;
            }
            else
            {
                height = thumbnailsize;
                width = thumbnailsize * originalImage.Width / originalImage.Height;
            }

            Bitmap thumbnailImage = null;
            try
            {
                thumbnailImage = new Bitmap(width, height);

                using (Graphics graphics = Graphics.FromImage(thumbnailImage))
                {
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }

                thumbnailImage.Save(output, ImageFormat.Jpeg);
            }
            finally
            {
                if (thumbnailImage != null)
                {
                    thumbnailImage.Dispose();
                }
            }
        }
    }
}
