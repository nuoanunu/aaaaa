using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SSM.Models
{
    public class Contactdata
    {
        public String FirstName { get; set; }
        public String lastname { get; set; }
        public String middlename { get; set; }
        public String email { get; set; }
        public String company { get; set; }
        public String phone { get; set; }
        public Contactdata(contact contact) {
            FirstName = contact.FirstName;
            lastname = contact.LastName;
            middlename = contact.MiddleName;
            this.email = contact.emails;
            this.phone = contact.Phone;
        }
    }
    public class dealdata {
        public Contactdata contact { get; set; }
        public taskdata DealTask { get; set; }
        public String productname { get; set; }
        public String date { get; set; }
    }
    public class taskdata {
        public String MailContent { get; set; }
        public String subject { get; set; }
        public taskdata(Plan_Step step) {
            MailContent = step.StepEmailContent;
            subject = step.subject;

        }

    }
}