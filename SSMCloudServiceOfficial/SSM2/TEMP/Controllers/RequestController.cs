using SSM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSM.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        // GET: Request
        public ActionResult Index()
        { SSMEntities se = new SSMEntities();
            List<Customer_Request> requestList = (se).Customer_Request.ToList();
            ViewData["NewRequest"] = requestList;

            int total = requestList.Count();
            int win = 0;
            int lost = 0;
            int unhandled = 0;
            int processing = 0;
            foreach (Customer_Request cr in requestList) {
               
                if (cr.DealID == null) unhandled = unhandled + 1;
                else if (cr.Deal.Status == 1) processing = processing + 1;
                else if (cr.Deal.Status == 4) lost = lost + 1;
                else if (cr.Deal.Status == 3 || cr.Deal.Status == 5) win = win + 1;
            }
            if (total != 0) {
                ViewData["WinRequest"] = win * 100 / total;
                ViewData["LostRequest"] = lost * 100 / total;
                ViewData["Unhandled"] = unhandled * 100 / total;
                ViewData["Processing"] = processing * 100 / total;
            }
            else {
                ViewData["WinRequest"] = 0;
                ViewData["LostRequest"] = 0;
                ViewData["Unhandled"] = 0;
                ViewData["Processing"] = 0;
            }
     
            return View("RequestPool");
        }
    }
}