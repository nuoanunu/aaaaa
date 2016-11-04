using SSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Owin;
using Microsoft.AspNet.Identity;
using System.Web.Script.Serialization;

namespace SSM.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        { String userID = User.Identity.GetUserId();
            if (User.IsInRole("manager"))
            {
                SSMEntities se = new SSMEntities();
                ViewData["cr"] = se.Customer_Request.Where(u => u.DealID == null).ToList();
                ViewData["pd"] = se.Deals.Where(u => u.Status == 3).ToList();
                ViewData["ll"] = se.productMarketPlans.Where(u => u.Licenses.Where(i => i.customerID == null).Count() < 10).ToList();
                ViewData["lt"] = se.productMarketPlans.Where(u => u.TrialAccounts.Where(i => i.contactID == null).Count() < 10).ToList();
                return View("ManagerDashboard");
            }
            else if (User.IsInRole("SalesRep")) {
                SSMEntities se = new SSMEntities();
                List<Deal_SaleRep_Respon> dll = se.Deal_SaleRep_Respon.Where(u => u.userID.Equals(userID)).ToList();
                List<DealTask> dt = new List<DealTask>();
                List<Deal> nd = new List<Deal>();
                foreach (Deal_SaleRep_Respon dsr in dll) {
                    dt.AddRange(dsr.Deal.DealTasks.Where(u => u.Deadline != null && u.type != 7 && u.type != 8).ToList());
                    if (dsr.Deal.StartDate == DateTime.Today) {
                        nd.Add(dsr.Deal);
                    }
                }
                ViewData["dt"] = dt.Where(u => (((DateTime)u.Deadline) - DateTime.Now).TotalDays <= 1).ToList();
                ViewData["nel"] = se.Licenses.Where(u => u.nextTransactionDate < DateTime.Now && u.SaleRepResponsible.Equals(userID)).ToList();
                ViewData["nd"] = nd;
                return View("SaleRepDashboard");
            }
            return View();
        }
        public class mynoti{
            public String title { get; set; }
            public String id { get; set; }
            public String des { get; set; }

            public String created { get; set; }
        }
        public JsonResult viewedNotification(int id)
        {
            SSMEntities se = new SSMEntities();
            Notification noti = se.Notifications.Find(id);
            if (noti != null) {
                noti.viewed = true;
                se.SaveChanges();
                return Json(new { succeed = noti.hreflink }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { ffail = "fail"}, JsonRequestBehavior.AllowGet);
        }
        public String getNewNotification()
        {
            SSMEntities se = new SSMEntities();
            AspNetUser user = se.AspNetUsers.Find(User.Identity.GetUserId());

            if (user != null) {
                JavaScriptSerializer js = new JavaScriptSerializer();


                List<Notification> notilist = user.Notifications.Where(u => !u.viewed).ToList();
                List<mynoti> resultlist = new List<mynoti>();
                foreach (Notification noti in notilist)
                {
                    mynoti no = new mynoti();
                    no.title = noti.NotiName.Trim();
                    no.created = noti.CreateDate.Day +"/"+ noti.CreateDate.Month + " " +noti.CreateDate.ToShortTimeString().Trim();
                    no.id = noti.id + "";
                    no.des = noti.NotiContent.Trim();
                    resultlist.Add(no);
                }
                return js.Serialize(resultlist);
            }
            return null;
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
//        public JsonResult gettop10noti()
//        {
//            SSMEntities se = new SSMEntities();
//            List< ManagerNotification> list =se.ManagerNotifications.SqlQuery(" SELECT TOP 6 * FROM ManagerNotification ORDER BY CreateDate DESC").ToList();
         
//            foreach (ManagerNotification noti in list) { }

//            return Json(new
//            {
//                items = new[] {
//    new {name = "command" , index = "X", optional = "0"},
//    new {name = "command" , index = "X", optional = "0"}
//}
//            }, JsonRequestBehavior.AllowGet);
//        }
    }
}