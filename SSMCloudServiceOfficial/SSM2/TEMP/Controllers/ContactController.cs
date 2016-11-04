using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SSM.Models;
using SSM.Models.Repository;
using SSM.Models.TempModel;

namespace SSM.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        contactRepository pr;
        // GET: Product
        public ContactController()
        {
            pr = new contactRepository(new SSMEntities());
        }
        public ActionResult Index()
        {
            ViewData["contactList"] = pr.getAll();
            return View("Contact");
        }

        public ActionResult NewContact()
        {
            var contactModel = new ContactEntity();
            return View("ContactInsertForm", contactModel);
        }

        [ValidateAntiForgeryToken]
        public ActionResult AddNewContact(ContactEntity contact)
        {
            if(!ModelState.IsValid)
            {
                return View("ContactInsertForm", contact);
            }
            contact dbContact = new Models.contact();
            dbContact.City = contact.City;
            dbContact.Comment = contact.Comment;
            dbContact.CompanyID = contact.CompanyID;
            dbContact.Country = contact.Country;
            dbContact.DateOfBirth = contact.DateOfBirth;
            dbContact.emails = contact.emails;
            dbContact.FirstName = contact.FirstName;
            dbContact.LastName = contact.LastName;
            dbContact.MiddleName = contact.MiddleName;
            dbContact.Phone = contact.Phone;
            dbContact.Photo = contact.Photo;
            dbContact.Region = contact.Region;
            dbContact.Sites = contact.Sites;
            dbContact.State = contact.State;
            dbContact.Street = contact.Street;
            dbContact.Zip = contact.Zip;

            pr.CreateNewContact(dbContact);
            return RedirectToAction("Index", "Contact");
        }

        
        public ActionResult EditContact(int id)
        {
            contact dbContact = pr.getById(id);

            ContactEntity contact = new ContactEntity();

            contact.City = dbContact.City;
            contact.Comment = dbContact.Comment;
            contact.CompanyID = dbContact.CompanyID;
            contact.Country = dbContact.Country;
            contact.DateOfBirth = dbContact.DateOfBirth;
            contact.emails = dbContact.emails;
            contact.FirstName = dbContact.FirstName;
            contact.LastName = dbContact.LastName;
            contact.MiddleName = dbContact.MiddleName;
            contact.Phone = dbContact.Phone;
            contact.Photo = dbContact.Photo;
            contact.Region = dbContact.Region;
            contact.Sites = dbContact.Sites;
            contact.State = dbContact.State;
            contact.Street = dbContact.Street;
            contact.Zip = dbContact.Zip;

            return View("ContactEditForm", contact);
        }

        [ValidateAntiForgeryToken]
        public ActionResult UpdateContact(ContactEntity contact)
        {
            if (!ModelState.IsValid)
            {
                return View("ContactEditForm", contact);
            }
            pr.EditContact(contact);
            return RedirectToAction("Index", "Contact");
        }
        public ActionResult LicenseUsing(int id) {
            SSMEntities se = new SSMEntities();
            contact con = se.contacts.Find(id);
            ViewData["Licenses"] = se.Licenses.Where(u => u.customer.cusEmail.Equals(con.emails)).ToList();
            ViewData["thiscontact"] = con;
            return View("ContactDetail");
        }
    }
}