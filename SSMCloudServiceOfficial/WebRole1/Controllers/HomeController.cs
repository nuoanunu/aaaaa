using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebRole1.Models;
using Microsoft.AspNet.Identity;
using WebRole1.Models.ContractModel;

namespace WebRole1.Controllers
{
    [Authorize(Roles = "Customer")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
       

            SSMEntities se = new SSMEntities();
            AspNetUser user = se.AspNetUsers.Find(User.Identity.GetUserId());
            if (user != null) {
                foreach (customer cus in user.customers.ToList())
                {
                    customer userprofile = cus;
                    if (userprofile != null)
                    {
                        ViewData["UnpaidOrder"] = se.orders.SqlQuery("SELECT * FROM orders where completedDate is null and customerID=" + userprofile.id).ToList();
                        ViewData["licenses"] = userprofile.Licenses.ToList();
                        ViewData["Payments"] = userprofile.Payments.ToList();
                        ViewData["profile"] = userprofile;
                    }
                }
           
            }
            Signer signer = new Signer();
            //signer.sign();
            return View();

        }

       
    }
}