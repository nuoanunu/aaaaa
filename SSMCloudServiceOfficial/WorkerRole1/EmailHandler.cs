using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    class EmailHandler
    {
        public static async Task SendMail(DealTask task)

        {
            SSMEntities se = new SSMEntities();
            var body = "{0}";
            var message = new MailMessage();
            message.To.Add(new MailAddress(task.Deal.contact.emails));   
            message.From = new MailAddress(se.ConfigureSys.Find(1).value);  
            message.Subject = task.TaskName ;

            message.Body = string.Format(body, Constant.replaceMailContent( task));
            message.IsBodyHtml = true;
 
            try {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = se.ConfigureSys.Find(1).value,  // replace with valid value
                        Password = se.ConfigureSys.Find(2).value // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

                }
            }
            catch (Exception e) {
                System.Diagnostics.Debug.WriteLine("hrereeee   Subject  " + e.InnerException.Message);
            }
            

        }
    }
}
