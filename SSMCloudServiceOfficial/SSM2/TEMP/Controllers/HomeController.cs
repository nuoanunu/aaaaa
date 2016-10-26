using SSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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