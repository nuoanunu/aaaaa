using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SSM.Models.TempModel
{
    public class EmailTemplateEntity
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Please enter email template's name")]
        [StringLength(255)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email content is empty")]
        public string MailContent { get; set; }

        public string creatorID { get; set; }
        public Nullable<System.DateTime> createdDate { get; set; }
        public Nullable<System.DateTime> lastUpdate { get; set; }
        public bool isActive { get; set; }
        public Nullable<int> CateID { get; set; }
       
        public EmailCategory EmailCategory { get; set; }

        [Display(Name = "Category")]
        [Required]
        public byte EmailCategoryId { get; set; }
    }
}