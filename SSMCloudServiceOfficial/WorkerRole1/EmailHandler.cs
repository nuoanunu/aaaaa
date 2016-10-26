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
            var body = "{0}";
            var message = new MailMessage();
            message.To.Add(new MailAddress(task.Deal.contact.emails));   
            message.From = new MailAddress("nhathn99@gmail.com");  
            message.Subject = task.TaskName ;

            message.Body = string.Format(body, Constant.replaceMailContent( task));
            message.IsBodyHtml = true;
 
            try {
                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "nhathn99@gmail.com",  // replace with valid value
                        Password = "320395qwe"  // replace with valid value
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
