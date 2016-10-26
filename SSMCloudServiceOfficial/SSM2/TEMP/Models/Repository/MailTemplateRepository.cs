using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSM.Models.TempModel;
using SSM.Models;

namespace SSM.Models.Repository
{
    public class MailTemplateRepository
    {

        SSMEntities context;

        public MailTemplateRepository(SSMEntities pContext)
        {
            context = pContext;
        }
        

        public List<Email_Template> getAll()
        {
            return context.Email_Template.ToList();
        }

        public void CreateNewEmailTemplate(EmailTemplateEntity template)
        {
            var dbTemplate = new Email_Template();

            dbTemplate.isActive = template.isActive;
            dbTemplate.lastUpdate = template.lastUpdate;
            dbTemplate.MailContent = template.MailContent;
            dbTemplate.Name = template.Name;
            dbTemplate.CateID = template.EmailCategoryId;
            dbTemplate.createdDate = template.createdDate;
            dbTemplate.creatorID = template.creatorID;            
            context.Email_Template.Add(dbTemplate);
            context.SaveChanges();
        }

        public void EditMailTemplate(EmailTemplateEntity template)
        {
            if (template == null)
                return;
            var dbTemplate = context.Email_Template.SingleOrDefault(c => c.id == template.id);
            if (dbTemplate == null)
                return;
            dbTemplate.isActive = template.isActive;
            dbTemplate.lastUpdate = template.lastUpdate;
            dbTemplate.MailContent = template.MailContent;
            dbTemplate.Name = template.Name;
            dbTemplate.CateID = template.CateID;
            dbTemplate.createdDate = template.createdDate;
            dbTemplate.creatorID = template.creatorID;   
            context.SaveChanges();
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

        public Email_Template getById(int id)
        {
            return context.Email_Template.Find(id);
        }
    }
}