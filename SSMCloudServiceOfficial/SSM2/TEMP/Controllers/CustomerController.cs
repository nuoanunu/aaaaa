using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SSM.Models;
using SSM.Models.Repository;

namespace SSM.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {

            return View("NewCustomer");
        }
        [HttpPost]
        public ActionResult CreateNewContact(contact model,String responsible)
        {
            customerRepository cusrepo = new customerRepository(new SSMEntities());
            cusrepo.CreateNewContact(model, responsible);
            return View("newCompany");
        }
        public ActionResult CreateNewCompany(company model, String responsible)
        {
            customerRepository cusrepo = new customerRepository(new SSMEntities());
            cusrepo.CreateNewCompany(model, responsible);
            return View("newCompany");
        }
        public ActionResult NewContact()
        {

            return View("CreateContact");
        }
    }

}