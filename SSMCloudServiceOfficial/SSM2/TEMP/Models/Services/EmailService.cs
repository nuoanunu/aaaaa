using SSM.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace SSM.Models.Services
{
    public class EmailServices
    {
        public static async Task SendMail(String mailcontent , String address,String subject )

        { DealRepository dealRepo = new DealRepository(new SSMEntities());
  
            var body = "{0}";
            var message = new MailMessage();
            message.To.Add(new MailAddress(address));  // replace with valid value 
            message.From = new MailAddress("dwarpro@gmail.com");  // replace with valid value
            message.Subject = subject;
            SSMEntities context = new SSMEntities();

            message.Body = string.Format(body, mailcontent);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "dwarpro@gmail.com",  // replace with valid value
                    Password = "320395qww"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
               await  smtp.SendMailAsync(message);
                System.Diagnostics.Debug.WriteLine("yay ?");
            }
      
        }
    }
}