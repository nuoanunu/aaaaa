using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models.Repository
{
    public class EmailCategoryRepository
    {
        SSMEntities context;

        public EmailCategoryRepository(SSMEntities pContext)
        {
            context = pContext;
        }

        public List<EMAIL_Category> getAll()
        {
            return context.EMAIL_Category.ToList();
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

        public EMAIL_Category CreateNewEmailCategory(EMAIL_Category emailCategory)
        {
            context.EMAIL_Category.Add(emailCategory);
            context.SaveChanges();
            return emailCategory;
        }

        public EMAIL_Category getById(int id)
        {
            return context.EMAIL_Category.Find(id);
        }
    }
}