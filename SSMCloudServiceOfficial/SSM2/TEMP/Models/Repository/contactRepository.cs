using SSM.Models.TempModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.Repository
{
    public class contactRepository: IDisposable
    {
        SSMEntities context;
        public contactRepository(SSMEntities pContext)
        {
            context = pContext;
        }
        public List<contact> getAll()
        {
            return context.contacts.ToList();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void CreateNewContact(contact contact)
        {  
            context.contacts.Add(contact);
            context.SaveChanges();        
        }
        
        public void EditContact(ContactEntity contact)
        {
            if (contact == null)
                 return;
            var dbContact = context.contacts.SingleOrDefault(c => c.id == contact.id);
            if (dbContact == null)
                return;
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

            context.SaveChanges();  
        }

        public contact getById(int id)
        {
            return context.contacts.Find(id);
        }
    }
}