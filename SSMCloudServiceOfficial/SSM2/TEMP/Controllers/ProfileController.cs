using Microsoft.AspNet.Identity;
using SSM.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSM.Models;
using SSM.Models.TempModel;
using System.Web.Security;
using Microsoft.Owin;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using SSM.Models.Storage;
using System.Web.Script.Serialization;

namespace SSM.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private ApplicationUserManager _userManager;
        // GET: Profilecombosample
        public ActionResult Index()
        {
            SSMEntities se = new SSMEntities();
            if(User.IsInRole("SalesRep") ){
                String userID = User.Identity.GetUserId();
                AspNetUser user = se.AspNetUsers.Find(userID);
                List<GrapDataSets> lst = new List<GrapDataSets>();
                List<String> color = new List<String> { "#0072BB", "#FF4C3B", "#FFD034", "#C6C8CA", "#0072BB", "#93228D" };
                foreach (Product_responsible pr in user.Product_responsible.ToList())
                {
                    GrapDataSets gds = new GrapDataSets();


                    gds.backgroundColor = color[0];
                    gds.data = new Random().Next(0, 100);
                    gds.label = pr.softwareProduct.name;
                    lst.Add(gds);
                    color.RemoveAt(0);

                }
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(lst);
                ViewData["SaleRepRevenue"] = json;
                ViewData["SalerepDetail"] = se.SaleRepProfiles.SqlQuery("SELECT * FROM SaleRepProfile where userID='" + userID + "'").FirstOrDefault();

                return View();
            }
            return RedirectToAction("Login", "Account");
        }
      
        public ActionResult SaleRepList(char? ln)
        {

            SSMEntities se = new SSMEntities();
            if (ln != null)
            {
               List<SaleRepProfile> lst = se.SaleRepProfiles.OrderByDescending(u=>u.AvarageKPI).ToList();
                List<SaleRepProfile> result = new List<SaleRepProfile>();
                foreach (SaleRepProfile srp in lst) {
                    if (srp.FullName.IndexOf((char)ln) == 0) {
                        result.Add(srp);
                    }
                }
                ViewData["ProfileList"] = result;

            }
            else
            {
                ViewData["ProfileList"] = se.SaleRepProfiles.OrderByDescending(u => u.AvarageKPI).ToList();

            }
            return View("SaleRepList");
        }
        public ActionResult Detail(int id)
        {
            SSMEntities se = new SSMEntities();
            SaleRepProfile profile = se.SaleRepProfiles.Find(id);
            AspNetUser user = profile.AspNetUser;
            List<GrapDataSets> lst = new List<GrapDataSets>();
            List<String> color = new List<String> { "#0072BB", "#FF4C3B", "#FFD034", "#C6C8CA", "#0072BB", "#93228D" };
            foreach (Product_responsible pr in user.Product_responsible.ToList())
            {
                GrapDataSets gds = new GrapDataSets();


                gds.backgroundColor = color[0];
                gds.data = new Random().Next(0, 100);
                gds.label = pr.softwareProduct.name;
                lst.Add(gds);
                color.RemoveAt(0);

            }
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(lst);
            ViewData["SaleRepRevenue"] = json;
            ViewData["SalerepDetail"] = se.SaleRepProfiles.SqlQuery("SELECT * FROM SaleRepProfile where userID='" + user.Id + "'").FirstOrDefault();

            return View();
        }
        public JsonResult paid(float money, int commisionID) {
            SSMEntities se = new SSMEntities();
            SaleRepCommision com = se.SaleRepCommisions.Find(commisionID);
            if (com != null) {
                if (money > 0) {
                    com.Paid = com.Paid + money;
                    se.SaveChanges();
                    return Json(new { result = "true" , paid = ((double)com.Paid).ToString("C"), percentage = com.Paid*100/com.Total }, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
        }
        public ActionResult ViewAllCommision()
        {
            SSMEntities se = new SSMEntities();
            ViewData["SaleRepCommision"] = se.SaleRepCommisions.Where(u => u.Paid < u.Total).ToList();
            return View("ComissionList");

        }
        public ActionResult Detail(String id)
        {
            SSMEntities se = new SSMEntities();
            ViewData["SalerepDetail"] = se.SaleRepProfiles.SqlQuery("SELECT * FROM SaleRepProfile where userID='" + id + "'").FirstOrDefault();
            return View("index");
        }
        public ActionResult Commision()
        {
            SSMEntities se = new SSMEntities();
            SaleRepProfile profile = se.SaleRepProfiles.SqlQuery("SELECT * FROM SaleRepProfile where userID='" + User.Identity.GetUserId() + "'").FirstOrDefault();
            ViewData["TotalContactMade"] = profile.AspNetUser.contact_resposible.Count();
            ViewData["SaleCycle"] = profile.SaleCycle;

            ViewData["Revenue"] = profile.AspNetUser.SaleRepCommisions.Sum(u => u.Total);
            ViewData["Collected"] = profile.AspNetUser.SaleRepCommisions.Sum(u => u.Paid);
            ViewData["TotalFromSubscription"] = profile.AspNetUser.SaleRepCommisions.Sum(u => u.MoneyFromSubcription);
            ViewData["TotalFromNewContract"] = profile.AspNetUser.SaleRepCommisions.Sum(u => u.MoneyFromNewContract);
            ViewData["TotalUnpaid"] =  (double)ViewData["Revenue"] - (double)ViewData["Collected"];
            return View("PersonalComission");
        }

        public ActionResult savechangesSchedule(String date)
        {
            SSMEntities se = new SSMEntities();
            String userID = User.Identity.GetUserId();
            AspNetUser user = se.AspNetUsers.Find(userID);
            string[] busyDates = date.Split(new[] { "start" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String thisdate in busyDates)
            {
                string[] startend = thisdate.Split(new[] { "end" }, StringSplitOptions.RemoveEmptyEntries);
                Calendar cal = new Calendar();
                cal.repeat = true;
                cal.startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                                .AddMilliseconds( double.Parse(startend[0]))
                                .ToLocalTime();
                cal.endTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                              .AddMilliseconds(double.Parse(startend[1]))
                              .ToLocalTime();
                cal.userID = userID;
                se.Calendars.Add(cal);
                se.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        public ActionResult NewProfile()
        {
            ProfileModel model = new ProfileModel();
            return View("CreateSaleRep", model);
        }
        public ActionResult CreateNewProfile(ProfileModel model)
        {
            if (ModelState.IsValid)
            {
                AccountController ac = new AccountController();
                String id = ac.RegisterNewAccount(model.username, model.password, System.Web.HttpContext.Current, "SalesRep");
                SaleRepProfile srp = new SaleRepProfile();
                srp.userID = id;
                srp.FirstName = model.firstname;
                srp.LastName = model.lastname;
                srp.FullName = model.firstname + " " + model.fullname + " " + model.lastname;
                srp.Email = model.email;
                srp.City = model.city;
                srp.Phone = model.phone;
                Storage storeage = new Storage();
                srp.Avatar = storeage.uploadMyfile(id, "avatar", model.avatar);
                srp.EmployedScine = DateTime.Today;
                srp.Country = model.country;
                srp.dateOfBirth = model.birthday;
                SSMEntities se = new SSMEntities();
                se.SaleRepProfiles.Add(srp);
                se.SaveChanges();
                return RedirectToAction("Detail", new { id = id });
            }
            return RedirectToAction("NewProfile");

        }
        public class GrapDataSets
        {
            public String label { get; set; }
            public String backgroundColor { get; set; }
            public int data { get; set; }
        }


    }


}