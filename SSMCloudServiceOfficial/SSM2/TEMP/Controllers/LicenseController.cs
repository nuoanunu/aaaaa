using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SSM.Controllers
{
    [Authorize]
    public class LicenseController : iController
    {
        // GET: License
        public ActionResult Index()
        {
            ViewData["licenseList"] = dbcnt.Licenses.ToList();
            return View("Manage");
        }
        public ActionResult licenseDetail(int id) {
            return View("LicenseDetil");
        }
    }
}