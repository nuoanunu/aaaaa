using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSM.Models;
using Microsoft.AspNet.Identity;
using Hangfire;
using SSM.Models.Services;
using SSM.Models.Repository;
using SSM.Models.Storage;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace SSM.Controllers
{
    [Authorize]
    public class DealController : Controller
    {
        // GET: Deal
        public ActionResult Index()
        {
            SSMEntities se = new SSMEntities();
            AspNetUser thisUSer = se.AspNetUsers.Find(User.Identity.GetUserId());
            if (thisUSer != null) {
                if (User.IsInRole("manager")) {
                    ViewData["ActiveDeal"] = se.Deals.OrderBy(u => u.Status).ToList();

                }
                else
                {
                    ViewData["ActiveDeal"] = thisUSer.Deal_SaleRep_Respon.Select(u => u.Deal).OrderBy(u => u.Status).ToList();
                }
            }

            return View("");
        }
        public ActionResult CreateDeal(Deal deal, int productID, int chooseplan)
        {
            int id = createAndGetDealID(deal, productID, chooseplan);

            return RedirectToAction("Detail", new { id = id });

        }
        [ValidateInput(false)]
        public ActionResult NewEmail(String mailcontent, int dealID)
        {
            SSMEntities se = new SSMEntities();
            Deal deal = se.Deals.Find(dealID);
            if (deal != null)
            {
                DealTask task = new Models.DealTask();
                task.type = 8;
                task.TaskName = deal.DealTasks.Where(u => u.type == 8).First().TaskName;
                task.status = 1;
                task.Deadline = DateTime.Now;
                task.CreateDate = DateTime.Now;
                task.TaskDescription = mailcontent;
                task.dealID = dealID;
                task.TaskContent = "ReplyEmail";
                se.DealTasks.Add(task);
                se.SaveChanges();
            }
            return RedirectToAction("Detail", new { id = dealID });

        }

        public int createAndGetDealID(Deal deal, int productID,int plann)
        {
            SSMEntities se = new SSMEntities();
            softwareProduct product = se.softwareProducts.Find(productID);
            if (product != null) deal.ProductID = productID;
            deal.Creator = User.Identity.GetUserId();
            deal.StartDate = DateTime.Today;
            deal.Stage = 1;
            deal.Probability = 0;
            deal.CompleteOn = null;
            deal.LastUpdateStage = null;
            deal.Status = 1;
            deal.CurrentPlanID = product.PrePurchase_FollowUp_Plan.Where(u => u.isOperation).FirstOrDefault().id;

            se.Deals.Add(deal);
            se.SaveChanges();
            Deal_Item ite = new Deal_Item();
            productMarketPlan pla = se.productMarketPlans.Find(plann);
            ite.planID = plann;
            ite.price = pla.ceilPrice;
            ite.Quantity = 1;
            ite.dealID = deal.id;
            se.Deal_Item.Add(ite);
            deal.Value = ite.price;
            se.SaveChanges();
            Deal_SaleRep_Respon dealrespont = new Deal_SaleRep_Respon();
            dealrespont.dealID = deal.id;
            dealrespont.userID = deal.Creator;
            se.Deal_SaleRep_Respon.Add(dealrespont);
            se.SaveChanges();
            contact contact = se.contacts.Find(deal.Client);
            int day = 0;
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
                    task.CreateDate = DateTime.Now;
                    task.TaskContent = tep.stepNo + "";
                    task.TaskName = tep.subject + " [#:" + deal.id + "]";
                    task.type = 8;
                    se.DealTasks.Add(task);
                    se.SaveChanges();
                    if (task.TaskDescription.Contains(se.ConfigureSys.Find(13).value))
                    {
                        String replaceall = "";
                        foreach (Deal_Item item in task.Deal.Deal_Item)
                        {
                            TrialAccount trial = item.productMarketPlan.TrialAccounts.Where(u => u.contactID == null).FirstOrDefault();
                            replaceall = replaceall + '\n' + "User Name for " + item.productMarketPlan.Name + ": " + trial.UserName + " Password: " + trial.Password;
                            TrialAccount trialupdate = se.TrialAccounts.Find(trial.AccountID);
                            trialupdate.contactID = task.Deal.contact.id;
                            se.SaveChanges();
                        }
                        task.TaskDescription = task.TaskDescription.Replace(se.ConfigureSys.Find(13).value, replaceall);
                    }
                    se.SaveChanges();
                }
            }


            return deal.id;

        }
        public JsonResult removeEmail(int mailid) {
            SSMEntities se = new SSMEntities();
            DealTask task = se.DealTasks.Find(mailid);
            if (task != null)
            {
                try {
                    
             
                    foreach (DealTask alltask in task.Deal.DealTasks.Where(u => u.type == 8 && u.Deadline > task.Deadline).ToList()) {
                        alltask.TaskContent = (int.Parse(alltask.TaskContent) - 1) +"";
                    }
                    se.DealTasks.Remove(task);
                    se.SaveChanges();
                    return Json(new { sentresult = "succeed" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e) {

                }
            }
            
            return Json(new { sentresult = "fail" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SendEarlyMail(int mailid) {
            SSMEntities se = new SSMEntities();
            DealTask task = se.DealTasks.Find(mailid);
            if (task != null) {
                if (task.Deal.Stage +1< (int.Parse(task.TaskContent)))
                {
                    return Json(new { sentresult = "fail" }, JsonRequestBehavior.AllowGet);
                }
                else {
                    task.Deadline = DateTime.Now;
                    se.SaveChanges();
                    return Json(new { sentresult = "suceed" }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { sentresult = "fail" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddItemToDeal(int planID, float suggestprice, int quantity, int dealID)
        {
            SSMEntities se = new SSMEntities();
            Deal deal = se.Deals.Find(dealID);
            if (deal != null)
            {
                Deal_Item item = new Deal_Item();
                item.planID = planID;
                item.dealID = dealID;
                item.price = suggestprice;
                item.Quantity = quantity;
                if (deal.Value == null)
                    deal.Value = suggestprice * quantity;
                else
                    deal.Value = deal.Value + suggestprice * quantity;
                se.Deal_Item.Add(item);
                se.SaveChanges();
            }
            return RedirectToAction("Detail", new { id = dealID });

        }
        public JsonResult getPlanMinMax(int planID)
        {
            SSMEntities se = new SSMEntities();
            productMarketPlan plan = se.productMarketPlans.Find(planID);
            if (plan != null)
            {
                return Json(new { min = plan.floorprice, max = plan.ceilPrice }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Detail(int id)
        {
            SSMEntities se = new SSMEntities();
            Deal thisDeal = se.Deals.Find(id);
            try
            {

                ViewData["DealDetail"] = thisDeal;

                List<SelectListItem> productPlan = new List<SelectListItem>();
                foreach (productMarketPlan plan in thisDeal.softwareProduct.productMarketPlans.ToList())
                {
                    productPlan.Add(new SelectListItem() { Text = plan.Name, Value = plan.id + "" });
                }

                ViewData["PlanList"] = productPlan;
                ViewData["TaskStatus"] = se.TaskStatus.ToList();
                ViewData["TaskTypes"] = se.TaskTypes.ToList();



                softwareProduct thisproduct = thisDeal.softwareProduct;
                int days = 0;
                foreach (PrePurchase_FollowUp_Plan plan in thisproduct.PrePurchase_FollowUp_Plan.ToList())
                {
                    if (plan.isOperation)
                    {
                        foreach (Plan_Step step in plan.Plan_Step)
                        {
                            if (step.TimeFromLastStep != null) days = days + (int)step.TimeFromLastStep;
                        }
                    }

                }
                System.Diagnostics.Debug.WriteLine("AAAAAAAAAAA " + days + " BBBBBB " + (DateTime.Today - thisDeal.StartDate).Days);
                ViewData["percentage"] = (DateTime.Today - thisDeal.StartDate).Days * 100 / days;


            }
            catch (Exception e) { }
            if (thisDeal.orders.Count() > 0)
            {

                return View("Detail", thisDeal.orders.FirstOrDefault());
            }
            return View("Detail");
        }
        public ActionResult CreateNewTask(int dealID, String title, String description, int type, String deadline)
        {
            try
            {
                SSMEntities se = new SSMEntities();
                DealTask task = new DealTask();
                task.dealID = dealID;
                task.Deadline = DateTime.Parse(deadline);
                task.CreateDate = DateTime.Today;
                task.status = 1;
                task.type = type;
                task.TaskDescription = description;
                task.TaskName = title;
                se.DealTasks.Add(task);
                se.SaveChanges();
                return RedirectToAction("Detail", new { id = dealID });
            }
            catch (Exception e)
            {
            }
            return RedirectToAction("Index");


        }
        public ActionResult Create()
        {
            List<SelectListItem> client = new List<SelectListItem>();
            List<SelectListItem> product = new List<SelectListItem>();

            //new SelectListItem() {Text="Alabama", Value="AL"}
            SSMEntities se = new SSMEntities();
            String userID = User.Identity.GetUserId();
            AspNetUser thissalerep = se.AspNetUsers.Find(userID);
            foreach (contact_resposible crm in thissalerep.contact_resposible.ToList())
            {
                client.Add(new SelectListItem() { Text = crm.contact.FirstName + " " + crm.contact.MiddleName + " " + crm.contact.LastName, Value = crm.contactID + "" });
            }
            foreach (Product_responsible plan in thissalerep.Product_responsible)
            {
                product.Add(new SelectListItem() { Text = plan.softwareProduct.name, Value = plan.softwareProduct.id + "" });             
            }

            ViewData["ProductResponsibleFor"] = product;
            ViewData["ClientResponsibleFor"] = client;
            ViewData["ProductPlan"] = se.productMarketPlans.ToList();
            return View("Create");
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult EditPlanMail(int taskid, String newcontent)
        {
            SSMEntities se = new SSMEntities();
            DealTask task = se.DealTasks.Find(taskid);
            try
            {
                if (task != null)
                {
                    task.TaskDescription = newcontent;
                    task.status = 1;
                    se.SaveChanges();
                    return Json(new { result = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e) { }

            return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
        }
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult DealWon(int id)
        {
            SSMEntities se = new SSMEntities();
            DealRepository dealrepo = new DealRepository(se);
            Deal deal = dealrepo.getByID(id);
            if (deal != null)
            {
                customer cus = new customer();
                if (se.AspNetUsers.Where(us => us.UserName.Equals(deal.contact.emails)).FirstOrDefault() == null)
                {
                    cus.cusAddress = deal.contact.Street + " " + deal.contact.City + " " + deal.contact.Region + " ";
                    cus.cusCompany = 1;
                    cus.cusEmail = deal.contact.emails;
                    cus.cusName = deal.contact.FirstName + " " + deal.contact.MiddleName + " " + deal.contact.LastName;
                    cus.cusPhone = deal.contact.Phone;
                    se.customers.Add(cus);
                    se.SaveChanges();
                    AccountController ac = new AccountController();
                    String cusAccountID = ac.RegisterNewAccount(cus.cusEmail, "320395@qwE", System.Web.HttpContext.Current, "Customer");
                    cus.userID = cusAccountID;
                    se.SaveChanges();
                }
                else
                {
                    AspNetUser thiscus = se.AspNetUsers.Where(us => us.UserName.Equals(deal.contact.emails)).First();
                    cus = thiscus.customers.First();
                }
                deal.Status = 3;

                order order = new order();
                order.customerID = cus.id;
                float price = 0;
                order.orderNumber = se.orders.ToList().Count() + 1;
                order.subtotal = deal.Value;
                order.status = 1;
                order.total = (double)order.subtotal * 1.1;
                order.VAT = (double)order.subtotal * 0.1;
                order.fromDeal = deal.id;
                order.createDate = DateTime.Now;
                Storage storeage = new Storage();
                se.orders.Add(order);
                se.SaveChanges();
                order.Contract = storeage.uploadfile(cus.userID, "order" + order.id);
                se.SaveChanges();
                deal.Status = 3;
                se.SaveChanges();
                bool validLicense = true;
                foreach (Deal_Item dealitem in deal.Deal_Item)
                {


                    int lcount = se.Licenses.Where(u => u.PlanID == dealitem.planID && u.status == null && u.SaleRepResponsible == null).Count();
                    if (lcount < dealitem.Quantity) validLicense = false;


                }
                if (validLicense)
                {
                    foreach (Deal_Item dealitem in deal.Deal_Item)
                    {

                        for (int i = 0; i < dealitem.Quantity; i++)
                        {
                            License license = se.Licenses.Where(u => u.PlanID == dealitem.planID && u.status == null && u.SaleRepResponsible == null).FirstOrDefault();
                            if (license != null)
                            {

                                license.customerID = cus.id;
                                license.SaleRepResponsible = deal.Deal_SaleRep_Respon.LastOrDefault().userID;
                                license.status = 1;
                                OrderItem orderItem = new OrderItem();
                                orderItem.orderID = order.id;
                                orderItem.planID = dealitem.planID;
                                orderItem.SoldPrice = (double)dealitem.price;
                                orderItem.LicenseID = license.id;
                                se.OrderItems.Add(orderItem);

                            }
                            else
                            {
                                return RedirectToAction("Detail", new {id=deal.id } );
                            }
                        }
                    }

                    se.SaveChanges();
                }
                else {
                    return RedirectToAction("Detail", new { id = deal.id });
                }
                Notification noti = new Notification();
                noti.NotiName = "Contract successfully created";
                noti.NotiContent = "Deal No." + deal.id + ": has finished with a contract and order";
                noti.userID = deal.Deal_SaleRep_Respon.LastOrDefault().userID;
                noti.CreateDate = DateTime.Now;
                noti.viewed = false;
                noti.hreflink = "/Deal/Detail?id=" + deal.id;
                se.Notifications.Add(noti);
                se.SaveChanges();
                deal.Status = 5;
                se.SaveChanges();
                //}
                //catch (Exception e) {
                //    return Json(new { result = "sad" }, JsonRequestBehavior.AllowGet);
                //}




            }
            return RedirectToAction("Detail", new { id = deal.id });
        }


    }
}