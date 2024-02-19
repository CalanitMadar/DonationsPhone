using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Helpers
{
    public class MailActions
    {
        public void SendMail(string filename)
        {
            

            var smtpClient = new SmtpClient("smtp.gmail.com",587)
            {
               
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Constants.FROMMAIL, Constants.MAILPASSWORD),
                EnableSsl = false,
            };
            


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(Constants.FROMMAIL );
            mailMessage.To.Add(Constants.TOMAIL );
            mailMessage.Subject = Constants.MAILSUBJECT  ;
            //mailMessage.Body = "This is test email";
            Attachment st = new Attachment(filename);
            mailMessage.Attachments.Add(st);
           

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Email Sent Successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }

    }
}
