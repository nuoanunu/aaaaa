using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.Repository
{
    public class customerRepository: IDisposable
    {
        SSMEntities context;
        public customerRepository(SSMEntities contextt) {
            context = contextt;
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
        public void CreateNewContact(contact cont, String responsible) {
            string[] stringSeparators = new string[] { ";" };
            String[] salerepid = responsible.Split(stringSeparators, StringSplitOptions.None);
     
            context.contacts.Add(cont);
            context.SaveChanges();
            for (int i = 0; i < salerepid.Length;i++) {
                try {
                    contact_resposible cr = new contact_resposible();
                    cr.contactID = cont.id;
                    cr.userID = salerepid[i];
                    context.contact_resposible.Add(cr);
                }
                catch (Exception e) { }
      
            }
            context.SaveChanges();

        }
        public void CreateNewCompany(company cont, String responsible)
        {
            string[] stringSeparators = new string[] { ";" };
            String[] salerepid = responsible.Split(stringSeparators, StringSplitOptions.None);

            context.companies.Add(cont);
            context.SaveChanges();
            for (int i = 0; i < salerepid.Length; i++)
            {
                try
                {
                    company_responsible cr = new company_responsible();
                    cr.companyID = cont.id;
                    cr.userID = salerepid[i];
                    context.company_responsible.Add(cr);
                }
                catch (Exception e) { }

            }
            context.SaveChanges();

        }

    }
}