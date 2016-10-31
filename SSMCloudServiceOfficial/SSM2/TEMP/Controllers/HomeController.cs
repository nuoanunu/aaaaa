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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
                    no.created = noti.CreateDate.ToShortTimeString().Trim();
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