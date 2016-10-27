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
using iTextSharp.text.pdf;
using System.IO;
using OpenPop.Pop3;
using OpenPop.Mime;
using Newtonsoft.Json.Linq;
using System.Data.Entity.Validation;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        SSMEntities se1;
        SSMEntities se2;
        SSMEntities se3;
        SSMEntities se4;
        SSMEntities se5;
        SSMEntities se6; SSMEntities se7;
        List<string> seenUids;
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {

            Thread.Sleep(10000);
            Trace.TraceInformation("WorkerRole1 is running");
            ThreadStart threadStart1 = new ThreadStart(handleRequest);
            ThreadStart threadStart2 = new ThreadStart(SaleRepRating);
            Thread th1 = new Thread(threadStart1);
            Thread th2 = new Thread(threadStart2);
            ThreadStart threadStart3 = new ThreadStart(TaskSendEmailFollowup);
            ThreadStart threadStart4 = new ThreadStart(inboxchecking);
            Thread th3 = new Thread(threadStart3);
            Thread th4 = new Thread(threadStart4);

            ThreadStart threadStart5 = new ThreadStart(UpdateLicense);
            Thread th5 = new Thread(threadStart5);
            ThreadStart threadStart6 = new ThreadStart(createOrderForIncomplete);
            Thread th6 = new Thread(threadStart6);
            ThreadStart threadStart7 = new ThreadStart(checkLiceneAndAccount);
            Thread th7 = new Thread(threadStart7);
            th1.Start();
            th2.Start();
            th3.Start();
            th5.Start();
            th4.Start();
            th6.Start();
            th7.Start();
            th1.Join();
            th2.Join();
            th5.Join();
            th3.Join();
            th4.Join();
            th6.Join();
            th7.Join();
            // TaskSendEmailFollowup();
            //SaleRepRating();

            //PdfReader pdfReader = null;
            //PdfStamper pdfStamper = null;

            //// Open the PDF file to be signed
            //pdfReader = new PdfReader(@"C:\Users\Nguyen Nhat\Downloads\Documents\draftsla_us.pdf");
            //if (pdfReader != null)
            //{
            //    // Output stream to write the stamped PDF to
            //    using (FileStream outStream = new FileStream(@"C:\Users\Nguyen Nhat\Downloads\Documents\draftsla_signed_us.pdf", FileMode.Create))
            //    {
            //        try
            //        {
            //            // Stamper to stamp the PDF with a signature
            //            pdfStamper = new PdfStamper(pdfReader, outStream);

            //            // Load signature image
            //            iTextSharp.text.Image sigImg = iTextSharp.text.Image.GetInstance(@"C:\Users\Nguyen Nhat\Downloads\Documents\donefixing.png");

            //            // Scale image to fit
            //            sigImg.ScaleToFit(210, 297);

            //            // Set signature position on page
            //            sigImg.SetAbsolutePosition(200, 200);

            //            // Add signatures to desired page
            //            PdfContentByte over = pdfStamper.GetOverContent(6);
            //            over.AddImage(sigImg);
            //        }
            //        finally
            //        {
            //            // Clean up
            //            if (pdfStamper != null)
            //                pdfStamper.Close();

            //            if (pdfReader != null)
            //                pdfReader.Close();

            //        }
            //    }
            //}
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();
            se1 = new SSMEntities();
            se2 = new SSMEntities();

            se3 = new SSMEntities();
            se4 = new SSMEntities();
            se5 = new SSMEntities();
            se6 = new SSMEntities();

            se7 = new SSMEntities();
            seenUids = new List<string>();
            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
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
        public void TaskSendEmailFollowup()
        {
            while (true)
            {


                DateTime before = (DateTime.Now.AddMinutes(-10));
                DateTime after = (DateTime.Now.AddMinutes(10));


                List<DealTask> lst = se1.DealTasks.Where(u => u.Deadline > before && u.Deadline < after && u.type == 8 && u.status == 1).ToList();

                foreach (DealTask task in lst)
                {

                    EmailHandler.SendMail(task);
                    task.status = 2;
                    if(!task.TaskContent.Equals("ReplyEmail"))
                    task.Deal.Stage = task.Deal.Stage + 1;
                    se1.SaveChanges();
                }
                Thread.Sleep(6000);
            }
        }
        public void handleRequest()
        {
            try { }
            catch (Exception e)
            {

            }
            while (true)
            {

                float totaltime = 0;
                List<Customer_Request> lst = se2.Customer_Request.Where(u => u.DealID == null).ToList();
                var requestDemoDate = new List<Tuple<DateTime, DateTime>>
                {

                };
                foreach (Customer_Request request in lst)
                {

                    JObject o = JObject.Parse(request.RequestDemoDay);
              
                        bool found = false;
                        DateTime startDate = new DateTime();
                        DateTime endDate = new DateTime();
                        foreach (JProperty p in o.Properties())
                        {
                            string name = p.Name;
                            string value = (string)p.Value;
                            if (p.Name.Equals("start"))
                            {
                                startDate = DateTime.Parse(value);
                                found = true;
                            }
                            if (p.Name.Equals("end"))
                            {
                                endDate = DateTime.Parse(value);
                            }

                        }
                        if (found)
                        {
                            requestDemoDate.Add(new Tuple<DateTime, DateTime>(startDate, endDate));
                            totaltime = totaltime + (float)(endDate - startDate).TotalHours;
                        }

                    
                    contact contact = request.contact;
                    softwareProduct software = request.productMarketPlan.softwareProduct;
                    List<Product_responsible> salereplst = software.Product_responsible.ToList();
                    Product_responsible pr = findSuitableSaleRep(salereplst, requestDemoDate, totaltime);
                    if (pr != null) {
                        Deal deal = new Deal();
                        deal.Creator = pr.saleRepID;
                        deal.Client = request.CusID;
                        deal.Status = 1;
                        deal.Stage = 1;
                        deal.StartDate = DateTime.Today;
                        deal.ProductID = request.productMarketPlan.softwareProduct.id;
                        deal.Name = "Request No." + request.id;
                        deal.Value = 0;
                        deal.CurrentPlanID = request.productMarketPlan.softwareProduct.PrePurchase_FollowUp_Plan.Where(u => u.isOperation).FirstOrDefault().id;

                        se2.Deals.Add(deal);
                        se2.SaveChanges();
                        Notification noti = new Notification();
                        noti.NotiName = "New Deal";
                        noti.NotiContent = contact.FirstName + " " + "has new request for " + deal.softwareProduct.name;
                        noti.userID = pr.saleRepID;
                        noti.CreateDate = DateTime.Now;
                        noti.viewed = false;
                        noti.hreflink = "/Deal/Detail?id=" + deal.id;
                        se2.Notifications.Add(noti);
                        se2.SaveChanges();
                        Deal_SaleRep_Respon respone = new Deal_SaleRep_Respon();
                        respone.dealID = deal.id;
                        respone.userID = pr.saleRepID;
                        se2.Deal_SaleRep_Respon.Add(respone);
                        se2.SaveChanges();
                        request.DealID = deal.id;
                        se2.SaveChanges();

                        int day = 0;
                        DealTask demodate = new DealTask();
                        demodate.CreateDate = DateTime.Now;
                        demodate.Deadline = requestDemoDate.First().Item1.AddHours(7);
                        demodate.dealID = deal.id;
                        demodate.type = 4;
                        demodate.status = 1;
                        demodate.TaskName = "Meeting with " + contact.FirstName;
                        demodate.TaskDescription = "Meeting with " + contact.FirstName + " for demo";
                        demodate.TaskContent = "";
                        se2.DealTasks.Add(demodate);
                        se2.SaveChanges();
                        PrePurchase_FollowUp_Plan plan = deal.softwareProduct.PrePurchase_FollowUp_Plan.Where(u => u.isOperation == true).FirstOrDefault();
                        if (contact != null)
                        {
                            foreach (Plan_Step tep in plan.Plan_Step)
                            {

                                if (tep.TimeFromLastStep == null) tep.TimeFromLastStep = 0;
                                day = day + (int)tep.TimeFromLastStep;
                                DealTask task = new DealTask();
                                task.dealID = deal.id;
                                task.TaskDescription = tep.StepEmailContent;
                                task.status = 1;
                                if (tep.RequireMoreDetail) task.status = 7;
                                task.Deadline = DateTime.Now.AddDays(day);
                                task.CreateDate = DateTime.Today;
                                task.TaskContent = tep.stepNo +"";
                                task.TaskName = tep.subject+ " [#:" + deal.id + "]";
                                task.type = 8;
                                se2.DealTasks.Add(task);
                                se2.SaveChanges();
                            }
                        }
                    }
                    
                }
                Thread.Sleep(1000);
            }
        }
        public void UpdateLicense() {
            while (true)
            {
                var groupedLicense = se5.Licenses.Where(u => u.nextTransactionDate == DateTime.Today).ToList().GroupBy(u => u.customerID);
                List<License> license = se5.Licenses.Where(u => u.nextTransactionDate == DateTime.Today).ToList();

                foreach (var group in groupedLicense)
                {
                    int userid = (int)group.Key;

                    order order = new order();
                    order.customerID = userid;
                    float price = 0;
                    order.orderNumber = se5.orders.Count() + 1;
                    order.subtotal = 0;
                    order.status = 1;
                    order.total = (double)order.subtotal * 1.1;
                    order.VAT = (double)order.subtotal * 0.1;
                    se5.orders.Add(order);
                    se5.SaveChanges();
                    double subtotal = 0;
                    String salerepID = "";
                    foreach (License lic in group)
                    {
                        

                        OrderItem orderItem = new OrderItem();
                        orderItem.orderID = order.id;
                        orderItem.planID = lic.PlanID;
                        orderItem.SoldPrice = lic.OrderItems.First().SoldPrice;
   
                        orderItem.LicenseID = lic.id;
                        lic.nextTransactionDate = ((DateTime)lic.nextTransactionDate).AddDays((double)lic.licenseDuration);

                        se5.OrderItems.Add(orderItem);
                        se5.SaveChanges();
                        subtotal = subtotal + orderItem.SoldPrice;
                        salerepID = lic.SaleRepResponsible;
                        SaleRepCommision commision = new SaleRepCommision();
                        commision.SaleRepID = salerepID;
                        commision.Total = orderItem.SoldPrice * 0.1;
                        commision.MoneyFromSubcription = orderItem.SoldPrice * 0.1;
                        commision.SaleRepID = salerepID;
                        commision.Paid = 0;
                        commision.DateIssue = DateTime.Today;
                        se5.SaleRepCommisions.Add(commision);
                        se5.SaveChanges();
                    }
                    order.subtotal = subtotal;
                    order.total = (double)order.subtotal * 1.1;
                    order.VAT = (double)order.subtotal * 0.1;
                    se5.SaveChanges();
                    
                }
                Thread.Sleep(60000);
            }
        }
        public Product_responsible findSuitableSaleRep(List<Product_responsible> salereplst, List<Tuple<DateTime, DateTime>> requestDemoDate, float totaltime)
        {
            if (salereplst.Count() > 0)
            {

                salereplst = salereplst.OrderBy(o => o.KPIforthisProduct).ToList();
                Tuple<DateTime, DateTime> tuple = requestDemoDate.First();
                DateTime start = tuple.Item1;
                DateTime end = tuple.Item2;
                start = tuple.Item1.AddHours(7);
                end = tuple.Item2.AddHours(7);
                System.Diagnostics.Debug.WriteLine("da tru  " + start + "  end" + end);
                System.Diagnostics.Debug.WriteLine("tuple  " + tuple.Item1 + "  end" + tuple.Item2);
                foreach (Product_responsible pr in salereplst)
                {
                    bool choosed = false;
                    AspNetUser salerep = pr.AspNetUser;
                    bool take = true;
                    foreach (Calendar cal in salerep.Calendars)
                    {
                       
                        if ( (int)cal.startTime.DayOfWeek == (int)tuple.Item1.DayOfWeek) {
                            System.Diagnostics.Debug.WriteLine("day of week  " + cal.startTime.DayOfWeek);
                            System.Diagnostics.Debug.WriteLine("cal.endTime.TimeOfDay  " + cal.startTime.TimeOfDay);
                            System.Diagnostics.Debug.WriteLine("day of week  " + cal.startTime.DayOfWeek);
                            if (cal.startTime.TimeOfDay < end.TimeOfDay && cal.endTime.TimeOfDay > start.TimeOfDay)
                            {
                                take = false;
                            }
                           
                        } 
                    }
        
                     if(take) return pr;
                }
            }
            return null;
        }
        public DateTime bigger(DateTime a, DateTime b)
        {
            if (a < b) return b;
            else return a;
        }

        public DateTime smaller(DateTime a, DateTime b)
        {
            if (a > b) return b;
            else return a;
        }
        public void SaleRepRating()
        {
            while (true)
            {

                List<SaleRepProfile> lst = se3.SaleRepProfiles.ToList();
                foreach (SaleRepProfile profile in lst)
                {
                    foreach (Product_responsible pr in profile.AspNetUser.Product_responsible.ToList())
                    {
                    
                            List<Deal> listdeal = se3.Deals.Where(u => u.ProductID == pr.productID && u.CompleteOn != null && ((DateTime)u.CompleteOn).Month == DateTime.Now.Month && ((DateTime)u.CompleteOn).Year == DateTime.Now.Year).ToList();
                            double t = 0;
                            foreach (Deal d in listdeal) { if(d.Value!=null) t = t + (double)d.Value; }
                            
                            if (t ==0 && listdeal.Count() == 0) pr.AvarageDealSize = 0;
                            else if(listdeal.Count() != 0) pr.AvarageDealSize = t / listdeal.Count();
                            se3.SaveChanges();
                            
                        
                       
             
                    }
                }
                    foreach (SaleRepProfile profile in lst)
                {
                    profile.newContactmade = profile.AspNetUser.contact_resposible.ToList().Where(u=> ((DateTime)u.createdDate).Month== DateTime.Now.Month && ((DateTime)u.createdDate).Year == DateTime.Now.Year).Count() / se3.contact_resposible.Where(u => ((DateTime)u.createdDate).Month == DateTime.Now.Month && ((DateTime)u.createdDate).Year == DateTime.Now.Year).Count();
                    List<Deal_SaleRep_Respon> lst2 = profile.AspNetUser.Deal_SaleRep_Respon.ToList();
                    float total = 0;
                    int allDeal = 0;
                    int winDeal = 0;
                    int numberofactivities = 0;
                    int salecycle = 0;
                    foreach (Deal_SaleRep_Respon dsr in lst2)
                    {
                  
                        if (dsr.Active)
                        {

                            allDeal = allDeal + 1;

                            if (dsr.Deal.Status == 3 || dsr.Deal.Status == 5)
                            {
                                total = total + (float)dsr.Deal.Value; winDeal = winDeal + 1;
                                numberofactivities = numberofactivities + dsr.Deal.DealTasks.ToList().Count();
                                salecycle = salecycle + ((((DateTime)dsr.Deal.CompleteOn - dsr.Deal.StartDate  ).Days));
                            }
                        }

                    }
                   if (allDeal != 0)
                    {
      


                        profile.SaleCycle = salecycle / allDeal;
                        profile.VolumeOfSales = total;
                        profile.winrate = winDeal / allDeal;
                        profile.avarageActivitiesPerdeal = numberofactivities / allDeal;
                        se3.SaveChanges();
                    }

                }
                SaleRepProfile profileTopAct;
                SaleRepProfile profileTopCycle;
                profileTopAct = se3.SaleRepProfiles.OrderByDescending(u => u.avarageActivitiesPerdeal).First();
                profileTopCycle = se3.SaleRepProfiles.OrderByDescending(u => u.SaleCycle).First();
                foreach (SaleRepProfile profile in lst)
                {
                    if(profileTopCycle.SaleCycle ==0) profile.SaleCycle = 1;
                    else
                    profile.SaleCycle =  (double)profile.SaleCycle / profileTopCycle.SaleCycle;
                    if (profileTopCycle.avarageActivitiesPerdeal == 0) profile.avarageActivitiesPerdeal = 1;
                    else
                    profile.avarageActivitiesPerdeal = profile.avarageActivitiesPerdeal / profileTopAct.avarageActivitiesPerdeal;
                    profile.AvarageKPI = 1- profile.SaleCycle + profile.avarageActivitiesPerdeal + profile.winrate + profile.newContactmade;
                    se3.SaveChanges();
                }

                foreach (SaleRepProfile profile in lst)
                {
                    foreach (Product_responsible pr in profile.AspNetUser.Product_responsible.ToList()) {
                        Product_responsible toppr = se3.Product_responsible.OrderByDescending(u => u.AvarageDealSize).First();
                        if (toppr.AvarageDealSize == 0) pr.KPIforthisProduct = profile.AvarageKPI + 1;
                        else
                        pr.KPIforthisProduct = profile.AvarageKPI +  (double)pr.AvarageDealSize / toppr.AvarageDealSize;
                        se3.SaveChanges();
                    }
                 
                }
                Thread.Sleep(1000);
            }
        }
        public void inboxchecking()
        {
            while (true)
            {
                try
                {
                    List<Message> newmessages = FetchUnseenMessages("pop.gmail.com", 995, true, "nhathn99@gmail.com", "320395qwe", seenUids);
        
                    foreach (Message mess in newmessages)
                    {
                        DealTask deal = new DealTask();

                        if (mess.Headers.Subject.Contains("[#:"))
                        {
                           String subject = mess.Headers.Subject;
                            if (subject.IndexOf("Re:") > -1) subject = subject.Replace("Re:", "").Trim();
                            String getdeal = subject.Substring(subject.IndexOf(":") + 1, subject.IndexOf("]") - subject.IndexOf(":") - 1);
                            int dealid = int.Parse(getdeal);
                            try
                            {
                              DealTask dt = new DealTask();
                                dt.CreateDate = DateTime.Now;
                                dt.Deadline = DateTime.Now;
                                dt.type = 7;
                                dt.TaskDescription = mess.ToMailMessage().Body;
                                dt.TaskName = subject;
                                dt.status = 1;
                                dt.dealID = dealid;
                                dt.TaskContent = "a";
                                se4.DealTasks.Add(dt);
                                  se4.SaveChangesAsync();
                            }
                            catch (DbEntityValidationException e)
                            {
                                var errorMessages = e.EntityValidationErrors
               .SelectMany(x => x.ValidationErrors)
               .Select(x => x.ErrorMessage);
                            }

                        }
                    }
                }
                catch (Exception e) { }
                Thread.Sleep(1000);
            }

        }

        public static List<Message> FetchUnseenMessages(string hostname, int port, bool useSsl, string username, string password, List<string> seenUids)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Fetch all the current uids seen
                List<string> uids = client.GetMessageUids();

                // Create a list we can return with all new messages
                List<Message> newMessages = new List<Message>();

                // All the new messages not seen by the POP3 client
                for (int i = 0; i < uids.Count; i++)
                {
                    string currentUidOnServer = uids[i];
                    if (!seenUids.Contains(currentUidOnServer))
                    {
                        // We have not seen this message before.
                        // Download it and add this new uid to seen uids

                        // the uids list is in messageNumber order - meaning that the first
                        // uid in the list has messageNumber of 1, and the second has 
                        // messageNumber 2. Therefore we can fetch the message using
                        // i + 1 since messageNumber should be in range [1, messageCount]
                        Message unseenMessage = client.GetMessage(i + 1);

                        // Add the message to the new messages
                        newMessages.Add(unseenMessage);

                        // Add the uid to the seen uids, as it has now been seen
                        seenUids.Add(currentUidOnServer);
                    }
                }

                // Return our new found messages
                return newMessages;
            }
        }
        public void checkLiceneAndAccount() {
            while (true)
            {
                foreach (productMarketPlan plan in se6.productMarketPlans.ToList())
                {
                    if (plan.Licenses.Count() < 50 && plan.Licenses.Count() > 10)
                    {
                        Notification noti = new Notification();
                        noti.NotiName = "Shortage licenses:50 left";
                        noti.NotiContent = "You need to create more licenses for plan: " + plan.Name + " of product " + plan.softwareProduct.name;
                        noti.userID = "3d23016d-074f-474e-a6b7-225de90b0cae";
                        noti.CreateDate = DateTime.Now;
                        noti.viewed = false;
                        noti.hreflink = "/Product/DetMarketPlanDetailail?id=" + plan.id;
                        if (se6.Notifications.Where(u => u.NotiContent.Equals(noti.NotiContent) && u.NotiName.Equals(noti.NotiName) && !u.viewed).FirstOrDefault() != null)
                        {
                            se6.Notifications.Add(noti);
                        }
                    }
                    else if (plan.Licenses.Count() <= 10)
                    {
                        Notification noti = new Notification();
                        noti.NotiName = "Shortage licenses:10 left";
                        noti.NotiContent = "You need to create more licenses for plan: " + plan.Name + " of product " + plan.softwareProduct.name;
                        noti.userID = "3d23016d-074f-474e-a6b7-225de90b0cae";
                        noti.CreateDate = DateTime.Now;
                        noti.viewed = false;
                        noti.hreflink = "/Product/DetMarketPlanDetailail?id=" + plan.id;
                        try {
                            if (se6.Notifications.Where(u => u.NotiContent.Equals(noti.NotiContent) && u.NotiName.Equals(noti.NotiName) && !u.viewed).FirstOrDefault() != null)
                            {
                                se6.Notifications.Add(noti);
                            }

                        }
                        catch (Exception e) { }
                       
                    }
                    if (plan.TrialAccounts.Count() < 50 && plan.TrialAccounts.Count() > 10)
                    {
                        Notification noti = new Notification();
                        noti.NotiName = "Shortage Trial Accounts:50 left";
                        noti.NotiContent = "You need to create more Trial Accounts for plan: " + plan.Name + " of product " + plan.softwareProduct.name;
                        noti.userID = "3d23016d-074f-474e-a6b7-225de90b0cae";
                        noti.CreateDate = DateTime.Now;
                        noti.viewed = false;
                        noti.hreflink = "/Product/DetMarketPlanDetailail?id=" + plan.id;
                        if (se6.Notifications.Where(u => u.NotiContent.Equals(noti.NotiContent) && u.NotiName.Equals(noti.NotiName) && !u.viewed).FirstOrDefault() != null)
                        {
                            se6.Notifications.Add(noti);
                        }
                    }
                    else if (plan.Licenses.Count() <= 10)
                    {
                        Notification noti = new Notification();
                        noti.NotiName = "Shortage Trial Accounts :10 left";
                        noti.NotiContent = "You need to create more Trial Accounts for plan: " + plan.Name + " of product " + plan.softwareProduct.name;
                        noti.userID = "3d23016d-074f-474e-a6b7-225de90b0cae";
                        noti.CreateDate = DateTime.Now;
                        noti.viewed = false;
                        noti.hreflink = "/Product/DetMarketPlanDetailail?id=" + plan.id;
                        if (se6.Notifications.Where(u => u.NotiContent.Equals(noti.NotiContent) && u.NotiName.Equals(noti.NotiName) && !u.viewed).FirstOrDefault() != null)
                        {
                            se6.Notifications.Add(noti);
                        }
                    }
                }
                se6.SaveChanges();
                Thread.Sleep(10000);
            }
        }
        public void createOrderForIncomplete() {
            while (true) {
                foreach (Deal deal in se7.Deals.Where(u => u.Status == 3).ToList())
                {
                    bool validLicense = true;
                    foreach (Deal_Item dealitem in deal.Deal_Item)
                    {
                        int lcount = se7.Licenses.Where(u => u.PlanID == dealitem.planID && u.status == null && u.SaleRepResponsible == null).Count();
                        if (lcount < dealitem.Quantity) validLicense = false;


                    }
                    if (validLicense)
                    {
                        foreach (Deal_Item dealitem in deal.Deal_Item)
                        {

                            for (int i = 0; i < dealitem.Quantity; i++)
                            {
                                License license = se7.Licenses.Where(u => u.PlanID == dealitem.planID && u.status == null && u.SaleRepResponsible == null).FirstOrDefault();
                                if (license != null)
                                {

                                    license.customerID = deal.orders.First().customerID;
                                    license.SaleRepResponsible = deal.Deal_SaleRep_Respon.LastOrDefault().userID;
                                    license.status = 1;
                                    OrderItem orderItem = new OrderItem();
                                    orderItem.orderID = deal.orders.First().id;
                                    orderItem.planID = dealitem.planID;
                                    orderItem.SoldPrice = (double)dealitem.price;
                                    orderItem.LicenseID = license.id;
                                    se7.OrderItems.Add(orderItem);

                                }
                            }
                        }

                        se7.SaveChanges();
                    }
                }
                Thread.Sleep(10000);
            }
            
        }
    }

}

