using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSM.Models;
using SSM.Models.Repository;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using SSM.Models.Services;
using SSM.Models.TempModel;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Entity;

namespace SSM.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        productRepository pr;
        // GET: Product
        public ProductController()
        {
            pr = new productRepository(new SSMEntities());
        }
        public ActionResult Index()
        {
            ViewData["productList"] = pr.getAll();
            return View("ProductList");
        }
        public ActionResult AttributeDetail(int id) {
            SSMEntities se = new SSMEntities();
            ViewData["attr"] = se.productAttributes.Find(id);
            return View("AttributeFragment");
        }
        public ActionResult FiltersLicense(int[] planIDs, int[] productIDs) {
            if (planIDs == null) planIDs = new int[0];
            if (productIDs == null) productIDs = new int[0];
            SSMEntities se = new SSMEntities();
            ViewData["TopPlan"] = (from plan in se.productMarketPlans
                                   orderby plan.Licenses.Where(u => u.customerID != null).Count() descending
                                   select plan
                                   ).Take(10).ToList();
            ViewData["NearExpire"] = se.Licenses.Where(u => u.nextTransactionDate != null && u.customerID != null && DbFunctions.DiffDays(u.nextTransactionDate, DateTime.Now) < 7 && (productIDs.Contains(u.productMarketPlan.softwareProduct.id)|| planIDs.Contains(u.PlanID))).ToList();
            ViewData["SoftwareList"] = se.softwareProducts.ToList();
            ViewData["PlanList"] = se.productMarketPlans.ToList();
            ViewData["LowList"] = se.productMarketPlans.Where(u => u.Licenses.Where(i => i.customerID == null).Count() < 10).ToList();
            return View("Licenses");
        }
        public ActionResult FiltersTrial(int[] planIDs, int[] productIDs)
        {
            if (planIDs == null) planIDs = new int[0];
            if (productIDs == null) productIDs = new int[0];
            SSMEntities se = new SSMEntities();
            ViewData["TopPlan"] = (from plan in se.productMarketPlans
                                   orderby plan.TrialAccounts.Where(u => u.contactID != null).Count() descending
                                   select plan
                                   ).Take(10).ToList();
            ViewData["NearExpire"] = se.TrialAccounts.Where(u => u.contactID != null && DbFunctions.DiffDays(u.enddate, DateTime.Now) < 7).ToList();
            ViewData["SoftwareList"] = se.softwareProducts.ToList();
            ViewData["PlanList"] = se.productMarketPlans.ToList();
            ViewData["LowList"] = se.productMarketPlans.Where(u => u.TrialAccounts.Where(i => i.contactID == null).Count() < 10).ToList();
            return View("TrialAccount");
        }
        public ActionResult Licenses() {
            SSMEntities se = new SSMEntities();
            ViewData["TopPlan"] = (from plan in se.productMarketPlans
                                    orderby   plan.Licenses.Where(u=> u.customerID!=null).Count() descending
                                   select plan
                                   ).Take(10).ToList();
            ViewData["NearExpire"] = se.Licenses.Where(u => u.nextTransactionDate != null && u.customerID!=null && DbFunctions.DiffDays(u.nextTransactionDate, DateTime.Now)<7).ToList();
            ViewData["SoftwareList"] = se.softwareProducts.ToList();
            ViewData["LicList"] = se.Licenses.ToList();
            ViewData["PlanList"] = se.productMarketPlans.ToList();
            ViewData["LowList"] = se.productMarketPlans.Where(u => u.Licenses.Where(i => i.customerID == null).Count() < 10).ToList();
            return View("Licenses");
        }
        public ActionResult TrialAccounts() {
            SSMEntities se = new SSMEntities();
            ViewData["TopPlan"] = (from plan in se.productMarketPlans
                                   orderby plan.TrialAccounts.Where(u => u.contactID != null).Count() descending
                                   select plan
                                   ).Take(10).ToList();
            ViewData["NearExpire"] = se.TrialAccounts.Where(u =>  u.contactID != null && DbFunctions.DiffDays(u.enddate, DateTime.Now) < 7).ToList();
            ViewData["SoftwareList"] = se.softwareProducts.ToList();
            ViewData["PlanList"] = se.productMarketPlans.ToList();
            ViewData["LowList"] = se.productMarketPlans.Where(u => u.TrialAccounts.Where(i => i.contactID == null).Count() < 10).ToList();
            ViewData["TriList"] = se.TrialAccounts.ToList();
            return View("TrialAccount");
        }
        public ActionResult MarketPlanDetail(int id) {
            SSMEntities se = new SSMEntities();
            productMarketPlan plan = se.productMarketPlans.Find(id);
            if (plan != null) {
                int totaluser = se.OrderItems.ToList().Count();
                int trailInuse = se.TrialAccounts.SqlQuery("SELECT * FROM TrialAccount WHERE planID= " + id + " and status=2").ToList().Count();
                int salers = se.Product_responsible.SqlQuery("SELECT * FROM Product_responsible WHERE productID=" + plan.productID).ToList().Count();

                List<Product_responsible> topssaler = se.Product_responsible.SqlQuery("SELECT * FROM Product_responsible WHERE productID=" + plan.productID).ToList();
                topssaler.OrderBy(x => x.TotalRevenue);
                if(topssaler.Count>6)
                topssaler.RemoveRange(6, topssaler.Count());
                List<OrderItem> lst = se.OrderItems.SqlQuery("SELECT * FROM OrderItems WHERE planID= " + plan.id).ToList();
                double total = 0;
                double thisplancontribute = 0;
                foreach (OrderItem pl in lst) {
                    total = total + pl.SoldPrice;
                    if (pl.id == plan.id) {
                        thisplancontribute = thisplancontribute + pl.SoldPrice;
                    }

                }
                
                double percentage = 0;
                if (total != 0) percentage = thisplancontribute * 100 / total;
                ViewData["trialAccount"] = se.TrialAccounts.SqlQuery("SELECT * FROM TrialAccount WHERE planID= " + id).ToList();
                ViewData["totaluser"] = totaluser;
                ViewData["trailInuse"] = trailInuse;
                ViewData["salers"] = salers;
                ViewData["percentage"] = percentage;
                ViewData["total"] = total;
                ViewData["topssaler"] = topssaler;
                ViewData["Plan"] = plan;
                ViewData["License"] = se.Licenses.Where(u => u.PlanID == id && u.customerID == null).ToList();
                return View("MarketPlanDetail", se.productMarketPlans.Find(id));
            }

            return View("Index");

        }
        [HttpPost]
        public ActionResult NewTrialAccount(String username, String password, int planID) {
            SSMEntities se = new SSMEntities();
            TrialAccount trail = new TrialAccount();
            trail.UserName = username;
            trail.Password = password;
            trail.PlanID = planID;
            trail.Status = 1;
            se.TrialAccounts.Add(trail);
            se.SaveChanges();
            return RedirectToAction("MarketPlanDetail", new { id = planID });
        }
        [HttpPost]
        public ActionResult NewLicense(String key, int duration, int planID, String url,String adminaccount, string adminpassword)
        {
            SSMEntities se = new SSMEntities();
            License lic = new License();
            lic.PlanID = planID;
            lic.licenseDuration = duration;
            lic.LicenseKey = key;
            lic.LinkUse = url;
            lic.AdminAccount = adminaccount;
            lic.AdminPassword = adminpassword;
            se.Licenses.Add(lic);
            se.SaveChanges();
            return RedirectToAction("MarketPlanDetail", new { id = planID });
        }
        public ActionResult NewPlan(int productID)
        {
            SSMEntities se = new SSMEntities();

            ViewData["MailCate"] = se.EMAIL_Category.ToList();
        
            ViewData["Product"] = productID;

            return View("NewFollowUpPlan", new FollowupProgressModel());
        }
        public ActionResult EditPlan(int planID)
        {
            SSMEntities se = new SSMEntities();
            ViewData["MailCate"] = se.EMAIL_Category.ToList();
            PrePurchase_FollowUp_Plan preplan = se.PrePurchase_FollowUp_Plan.Find(planID);
            ViewData["planID"] = preplan.id;
            ViewData["plan"] = preplan;
            ViewData["Product"] = preplan.softwareProduct.id;
            ViewData["steps"] = preplan.Plan_Step.ToList();
            preplan.productID = planID;
            // return View("EditFollowUpPlan");
            return View("FollowUpProgressEditor", new FollowupProgressModel(preplan));
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNewProgress(FollowupProgressModel model) {
            PrePurchase_FollowUp_Plan plan = new PrePurchase_FollowUp_Plan();
            plan.Description = model.Desription;
            plan.name = model.Name;
            plan.isActive = true;
            plan.isOperation = false;
            plan.productID = model.productID;

            plan.createDate = DateTime.Today;
            
            SSMEntities se = new SSMEntities();
            se.PrePurchase_FollowUp_Plan.Add(plan);
            se.SaveChanges();

            foreach (Plan_Step step in model.steps.ToList()) {
                if (step.StepEmailContent == null) model.steps.Remove(step);
                else if (step.StepEmailContent.Trim().Length ==0) model.steps.Remove(step);
            }
            for (int i = 0; i < model.steps.Count(); i++) {
                Plan_Step step = model.steps[i];
                step.stepNo = (i + 1);
                step.planID = plan.id;
                se.Plan_Step.Add(step);
                se.SaveChanges();
            }
            for (int i = 0; i < model.steps.Count(); i++)
            {
                Plan_Step step = model.steps[i];
                if (i != 0) step.previousStep = model.steps[i - 1].id;
                if (i != model.steps.Count()-1) step.nextStep = model.steps[i + 1].id;
                se.SaveChanges();
            }

            return RedirectToAction("Detail" , new { id = model.productID});
        }

        [HttpPost]
        public ActionResult SetInactive(int planid, int productID)
        {
            try
            {
                SSMEntities se = new SSMEntities();
                PrePurchase_FollowUp_Plan plan = se.PrePurchase_FollowUp_Plan.Find(planid);
                plan.isActive = false;
                se.SaveChanges();
                System.Diagnostics.Debug.WriteLine("cccccccc " + productID);
                return RedirectToAction("Detail", new { id = productID });
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Index");

        }
        public ActionResult SetInactiveMarketPlan(int planid, int productID)
        {
            try
            {
                SSMEntities se = new SSMEntities();
                productMarketPlan plan = se.productMarketPlans.Find(planid);
                plan.isActive = false;
                se.SaveChanges();
                System.Diagnostics.Debug.WriteLine("cccccccc " + productID);
                return RedirectToAction("Detail", new { id = productID });
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Index");

        }
        public ActionResult Detail(int id)
        {
           productRepository pr = new productRepository(new SSMEntities());
            try
            {
                softwareProduct product = pr.getById(id);
                if (product != null)
                {
                    List<double> totalvalues = new List<double>();
                    ProductServices ps = new ProductServices();
                    for (int i = 0; i < 12; i++)
                    {
                        totalvalues .Add( ps.getMonthValues(i, product.id));
                        System.Diagnostics.Debug.WriteLine("added " + ps.getMonthValues(i, product.id))  ;
                    }
                    ViewData["productDetail"] = product;

                    ViewData["productperformance"] = totalvalues;
                    return View("ProductDescription");
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");

        }
        public JsonResult CreateNewOption(int attID, String optCode, String optname, String optdes, float optprice)
        {
            try
            {
                attributeOption opt = new attributeOption();
                opt.attributeID = attID;
                opt.code = optCode;
                opt.name = optname;
                opt.price = optprice;
                opt.description = optdes;
                productRepository se = new productRepository(new SSMEntities());
                se.insertNewOption(opt);

                return Json(new { result = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { result = "false" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateNewAtt(int productID, String attname, String attcode, String attdes)
        {
            try
            {
                productAttribute att = new productAttribute();
                att.name = attname;
                att.productID = productID;
                att.code = attcode;
                att.description = attcode;
                productRepository se = new productRepository(new SSMEntities());
                se.insertNewAttribute(att);

                return Json(new { result = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { result = "false" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateNewMarketPlan(int productID, String planName, float price, float floorPrice, float ceilPrice, String optionIds, String newMarketPlanDes)
        {
            try
            {
                productMarketPlan att = new productMarketPlan();
                att.Name = planName;
                att.productID = productID;
                att.Description = newMarketPlanDes;
                att.Price = price;
                att.floorprice = floorPrice;
                att.ceilPrice = ceilPrice;
                att.isActive = true;
                att.operating = false;
                att.CreatedDay = DateTime.Today;
                string[] ids = optionIds.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                productRepository se = new productRepository(new SSMEntities());
                se.insertNewMarketPlan(att, ids);

                return Json(new { result = "true" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { result = "false" }, JsonRequestBehavior.AllowGet);

        }
        public async Task<ActionResult> guimail()
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress("nhatvhn99@gmail.com"));  // replace with valid value 
            message.From = new MailAddress("dwarpro@gmail.com");  // replace with valid value
            message.Subject = "Your email subject";
            SSMEntities context = new SSMEntities();

            message.Body = string.Format(body, "dwarpro@gmail.com", "dwarpro@gmail.com", context.Email_Template.ToList().First().MailContent);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "dwarpro@gmail.com",  // replace with valid value
                    Password = "320395qww"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);
                return Json(new { success = " suc" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = " fale" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult PrePlanSwitchStatus(int id)
        {
            try
            {
                SSMEntities se = new SSMEntities();
                PrePurchase_FollowUp_Plan myplan = se.PrePurchase_FollowUp_Plan.Find(id);
                softwareProduct software = myplan.softwareProduct;
                foreach (PrePurchase_FollowUp_Plan plan in software.PrePurchase_FollowUp_Plan.ToList())
                {
                    if (plan.id != id)
                        plan.isOperation = false;
                }
                myplan.isOperation = !myplan.isOperation;
                se.SaveChanges();
                return Json(new { result = "succeed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult MarketPlanSwitchStatus(int id)
        {
            try
            {
                SSMEntities se = new SSMEntities();
                productMarketPlan myplan = se.productMarketPlans.Find(id);
                myplan.operating = !myplan.operating;
                se.SaveChanges();
                return Json(new { result = "succeed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

            }
            return Json(new { result = "fail" }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PlanTrialImport(HttpPostedFileBase excelfile,int planID)
        {
            SSMEntities se = new SSMEntities();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select an Excel file<br>";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    System.Diagnostics.Debug.WriteLine("noa ne: "+path);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    // Read data from excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<TrialAccount> listAccounts = new List<TrialAccount>();
                    // row = 3 is the row data begin with - 1
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        TrialAccount account = new TrialAccount();
                        account.UserName = ((Excel.Range)range.Cells[row, 1]).Text;
                        account.Password = ((Excel.Range)range.Cells[row, 2]).Text;
                        account.PlanID = planID;
                        account.Status = 1;
                       
                        //account.PlanID = int.Parse(((Excel.Range)range.Cells[row, 3]).Text);
                        //DateTime createdDate = Convert.ToDateTime(((Excel.Range)range.Cells[row, 4]).Text);
                        //account.createdDate = createdDate;
                        se.TrialAccounts.Add(account);
                    }
                    application.Workbooks.Close();
                    se.SaveChanges();
                    
                    return RedirectToAction("MarketPlanDetail", new { id = planID });
                }
                else
                {
                    return RedirectToAction("MarketPlanDetail", new { id = planID});
                }

            }

        }
        public ActionResult LicenseImport(HttpPostedFileBase excelfile, int planID)
        {
            SSMEntities se = new SSMEntities();
            if (excelfile == null || excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select an Excel file<br>";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string path = Server.MapPath("~/Content/" + excelfile.FileName);
                    System.Diagnostics.Debug.WriteLine("noa ne: " + path);
                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                    excelfile.SaveAs(path);
                    // Read data from excel file
                    Excel.Application application = new Excel.Application();
                    Excel.Workbook workbook = application.Workbooks.Open(path);
                    Excel.Worksheet worksheet = workbook.ActiveSheet;
                    Excel.Range range = worksheet.UsedRange;
                    List<TrialAccount> listAccounts = new List<TrialAccount>();
                    // row = 3 is the row data begin with - 1
                    for (int row = 3; row <= range.Rows.Count; row++)
                    {
                        License lic = new License();
                        lic.LicenseKey = ((Excel.Range)range.Cells[row, 1]).Text;
                        lic.PlanID = planID;
                        lic.licenseDuration = ((Excel.Range)range.Cells[row, 2]).Text;
                        lic.LinkUse = ((Excel.Range)range.Cells[row, 3]).Text;
                        lic.AdminAccount = ((Excel.Range)range.Cells[row, 4]).Text;
                        lic.AdminPassword = ((Excel.Range)range.Cells[row, 5]).Text;
                        //account.PlanID = int.Parse(((Excel.Range)range.Cells[row, 3]).Text);
                        //DateTime createdDate = Convert.ToDateTime(((Excel.Range)range.Cells[row, 4]).Text);
                        //account.createdDate = createdDate;
                        se.Licenses.Add(lic);
                    }
                    application.Workbooks.Close();
                    se.SaveChanges();

                    return RedirectToAction("MarketPlanDetail", new { id = planID });
                }
                else
                {
                    return RedirectToAction("MarketPlanDetail", new { id = planID });
                }

            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult FinishEditProgress(FollowupProgressModel model) {
            SSMEntities se = new SSMEntities();
            PrePurchase_FollowUp_Plan plan = se.PrePurchase_FollowUp_Plan.Find(model.id);
            if (plan != null) {
                plan.name = model.Name;
                plan.Description = model.Desription;
                plan.lastUpdate = DateTime.Today;
                foreach (Plan_Step step in plan.Plan_Step.ToList()) {
                    step.previousStep = null;
                    step.nextStep = null;
                    se.SaveChanges();
                    se.Plan_Step.Remove(step);
                }
                int index = 1;
                for (int i = 1; i < model.steps.Count()+1; i++) {

                    Plan_Step step = model.steps[i - 1];
                    if (step.StepEmailContent != null)
                    {
                        step.planID = plan.id;
                        step.stepNo = i;

                        se.Plan_Step.Add(step);
                        se.SaveChanges();
                        index = index + 1;
                    }
                    else {
                        model.steps.Remove(step);
                    }
                }
                for (int i = 1; i < model.steps.Count() + 1; i++)
                {
                    Plan_Step step = model.steps[i - 1];
                    try {
                        step.previousStep = model.steps[i - 2].id;
                    }
                    catch (Exception e) { step.previousStep = null; }
                    try
                    {
                        step.nextStep = model.steps[i ].id;
                    }
                    catch (Exception e) { step.nextStep = null; }
                    try
                    {
                        if (step.previousStep == 0) step.previousStep = null;
                    }
                    catch (Exception e) { }
                    try
                    {
                        if (step.nextStep == 0) step.nextStep = null;
                    }
                    catch (Exception e) { }
                    se.SaveChanges();
                }
            }
            return RedirectToAction("EditPlan", new { planID = plan.id });
        }
        
    }
}