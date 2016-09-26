using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniversityDbCommon.DAL;
using UniversityDbCommon.Models;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace UniversityDbWeb.Controllers
{
    public class StudentController : Controller
    {
        private UniversityDbContext db = new UniversityDbContext();
        private CloudQueue imagesQueue;
        private static CloudBlobContainer imagesBlobContainer;

        public StudentController()
        {
            InitializeStorage();
        }

        private void InitializeStorage()
        {
            var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));

            var blobClient = storageAccount.CreateCloudBlobClient();
            blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);
            imagesBlobContainer = blobClient.GetContainerReference("university-images");

            var queueClient = storageAccount.CreateCloudQueueClient();
            queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);
            imagesQueue = queueClient.GetQueueReference("university-images");
        }

        // GET: Student
        public ActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "LastName,FirstMidName,EnrollmentDate")] Student student,
            HttpPostedFileBase imageFile)
        {
            CloudBlockBlob imageBlob = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.ContentLength != 0)
                    {
                        imageBlob = await UploadAndSaveBlobAsync(imageFile);
                        student.ProfileImageUrl = imageBlob.Uri.ToString();
                    }
                    db.Students.Add(student);
                    await db.SaveChangesAsync();
                    Trace.TraceInformation("Created Student {0} in database", student.ID);

                    if (imageBlob != null)
                    {
                        var queueMessage = new CloudQueueMessage(student.ID.ToString());
                        await imagesQueue.AddMessageAsync(queueMessage);
                        Trace.TraceInformation("Created queue message for Student {0}", student.ID);
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,LastName,FirstMidName,EnrollmentDate")] Student student,
            HttpPostedFileBase imageFile)
        {
            CloudBlockBlob imageBlob = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (imageFile != null && imageFile.ContentLength != 0)
                    {
                        await DeleteBlobsAsync(student);
                        imageBlob = await UploadAndSaveBlobAsync(imageFile);
                        student.ProfileImageUrl = imageBlob.Uri.ToString();
                    }
                    db.Entry(student).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Trace.TraceInformation("Updated Student {0} in database", student.ID);

                    if (imageBlob != null)
                    {
                        var queueMessage = new CloudQueueMessage(student.ID.ToString());
                        await imagesQueue.AddMessageAsync(queueMessage);
                        Trace.TraceInformation("Created queue message for Student {0}", student.ID);
                    }
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                Student student = await db.Students.FindAsync(id);

                await DeleteBlobsAsync(student);

                db.Students.Remove(student);
                await db.SaveChangesAsync();
            }
            catch (DataException)
            {
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }

            return RedirectToAction("Index");
        }

        private async Task<CloudBlockBlob> UploadAndSaveBlobAsync(HttpPostedFileBase imageFile)
        {
            Trace.TraceInformation("Uploading image file {0}", imageFile.FileName);

            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            CloudBlockBlob imageBlob = imagesBlobContainer.GetBlockBlobReference(blobName);
            using (var fileStream = imageFile.InputStream)
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
            }

            Trace.TraceInformation("Uploaded image file to {0}", imageBlob.Uri.ToString());

            return imageBlob;
        }

        private async Task DeleteBlobsAsync(Student student)
        {
            if (!string.IsNullOrWhiteSpace(student.ProfileImageUrl))
            {
                Uri blobUri = new Uri(student.ProfileImageUrl);
                await DeleteBlobAsync(blobUri);
            }
            if (!string.IsNullOrWhiteSpace(student.ProfileThumbnailUrl))
            {
                Uri blobUri = new Uri(student.ProfileThumbnailUrl);
                await DeleteBlobAsync(blobUri);
            }
        }

        private static async Task DeleteBlobAsync(Uri blobUri)
        {
            string blobName = blobUri.Segments[blobUri.Segments.Length - 1];
            Trace.TraceInformation("Deleting image blob {0}", blobName);
            CloudBlockBlob blobToDelete = imagesBlobContainer.GetBlockBlobReference(blobName);
            await blobToDelete.DeleteAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
