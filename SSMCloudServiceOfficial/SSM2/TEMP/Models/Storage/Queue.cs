using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.Storage
{
    public class Queue
    {
        public void Create(String messagee) {
            CloudStorageAccount ac = CloudStorageAccount.Parse("UseDevelopmentStorage=true");
            CloudQueueClient qclient = ac.CreateCloudQueueClient();
            CloudQueue que = qclient.GetQueueReference("NewContract");
            que.CreateIfNotExists();
            CloudQueueMessage message = new CloudQueueMessage(messagee);
            que.AddMessage(message);
        }
    }
}