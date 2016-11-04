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
        public static async Task SendMail(DealTask task,SSMEntities se)

        {
            var body = "{0}";
            System.Diagnostics.Debug.WriteLine("body:[" + se.ConfigureSys.Find(1).value.Trim());

            var message = new MailMessage();
            message.To.Add(new MailAddress(task.Deal.contact.emails));
            System.Diagnostics.Debug.WriteLine("aaa:[" + se.ConfigureSys.Find(1).value.Trim());

            message.From = new MailAddress(se.ConfigureSys.Find(1).value.Trim());  
            message.Subject = task.TaskName ;
            Constant emailchanger = new Constant(se);
            message.Body = emailchanger.replaceMailContent(task);
            message.IsBodyHtml = true;
            System.Diagnostics.Debug.WriteLine("username:[" + se.ConfigureSys.Find(1).value.Trim() + "] pass:[" + se.ConfigureSys.Find(2).value.Trim() + "]");


                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = se.ConfigureSys.Find(1).value.Trim(),  // replace with valid value
                        Password = se.ConfigureSys.Find(2).value.Trim() // replace with valid value
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);

                }
           
            

        }
    }
}
