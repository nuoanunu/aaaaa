using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SSM.Models;
using SSM.Models.Repository;
using SSM.Models.TempModel;


namespace SSM.ViewModels
{
    public class CreateEmailTemplateViewModel
    {
        public IEnumerable<EmailCategory> EmailCategories { get; set; }
        public EmailTemplateEntity EmailTemplateEntity { get; set; }
        public EmailCategory EmailCategory { get; set; }
    }
}