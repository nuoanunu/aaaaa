using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SiteBanHang.Models;
namespace SiteBanHang.Controllers
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
        public ActionResult PlanList()
        {
            SSMEntities se = new SSMEntities();

            List<productMarketPlan> lst = se.productMarketPlans.SqlQuery("SELECT * FROM productMarketPlan WHERE operating =1").ToList();
            List<softwareProduct> lst2 = se.softwareProducts.ToList();
            ViewData["ProductList"] = lst2;
            ViewData["PlanList"] = lst;
            return View("MarketPlan");
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
        public ActionResult newRequest(int planid)
        {
            ViewData["planID"] = planid;
            SSMEntities se = new SSMEntities();
            productMarketPlan plan = se.productMarketPlans.Find(planid);
            int salerepresponse = plan.softwareProduct.Product_responsible.Count();

            List<Calendar> blockeddate = se.Calendars.Where(u => u.repeat).ToList();
            if (blockeddate.Count() > 0)
            {
                System.Diagnostics.Debug.WriteLine("cc");
                Calendar core = blockeddate.First();
                List<Calendar> coreDate = blockeddate.Where(u => u.userID.Equals(core.userID)).ToList();
                List<Calendar> BlockedDate = new List<Calendar>();
                var dates = new List<Tuple<DateTime, DateTime>>();

                foreach (Calendar corecal in coreDate)
                {
                    int meet = 0;
                    foreach (Calendar cal in blockeddate)
                    {

                        if (cal != corecal)
                        {
                            if (cal.startTime <= corecal.endTime && cal.endTime >= corecal.startTime)
                            {
                                meet = meet + 1;
                                corecal.startTime = bigger(cal.startTime, corecal.startTime);
                                corecal.endTime = smaller(cal.endTime, corecal.endTime);
                            }
                        }
                    }
                    System.Diagnostics.Debug.WriteLine("mett " + meet + " res " + salerepresponse);
                    if (meet == salerepresponse -1)
                    {
                        BlockedDate.Add(corecal);
                    }

                }

                if (BlockedDate.Count() > 0)
                {
                    ViewData["unavailabledate"] = BlockedDate;
                }
                
            }
            else {
                ViewData["unavailabledate"] = null;
            }

            ViewData["repcount"] = salerepresponse;
            return View("ScheduleDemo");
        }
        public ActionResult makenewRequest(int planID, String firstname, String lastname, String email, String businessname, String BussinessAddress, String website, String role, String description, String dates)
        {
            System.Diagnostics.Debug.WriteLine("em ay day "  +dates);
            SSMEntities se = new SSMEntities();
            productMarketPlan plan = se.productMarketPlans.Find(planID);
            contact con = new contact();
            con.FirstName = firstname;
            con.MiddleName = " ";
            con.Phone = "Not available";
            con.Photo = "Not available";
            con.DateOfBirth = DateTime.Today;
            con.Street = BussinessAddress;
            con.LastName = lastname;
            con.emails = email;
            con.Comment = description;
            company com = new company();
            com.companyName = businessname;
            com.Street = BussinessAddress;
            com.logo = "Not available";
            com.sites = website;
            se.companies.Add(com);
            se.SaveChanges();
            con.CompanyID = com.id;
            se.contacts.Add(con);
            se.SaveChanges();
            Customer_Request request = new Customer_Request();
            request.RequestNo = con.id + "" + com.id + "";
            request.PlanID = planID;
            request.ProductID = plan.softwareProduct.id;
            request.CusID = con.id;
            request.CreatedDate = DateTime.Today;
            request.RequestDemoDay = dates;
            se.Customer_Request.Add(request);
            se.SaveChanges();
            Notification noti = new Notification();
            noti.NotiName = "New Request";
            noti.NotiContent = con.FirstName + " " + "has new request for " + plan.softwareProduct.name;
            noti.userID = "3d23016d-074f-474e-a6b7-225de90b0cae";
            noti.CreateDate = DateTime.Now;
            noti.viewed = false;
            noti.hreflink = "/Request/";
            se.Notifications.Add(noti);
            se.SaveChanges();
            return View("Success");
        }

    }
}