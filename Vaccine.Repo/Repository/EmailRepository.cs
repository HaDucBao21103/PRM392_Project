using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Vaccine.Repo.Repository
{
    public static class EmailRepository
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("thunm300104@gmail.com");
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new NetworkCredential("thunm300104@gmail.com", "pyzy ogej ysgq criv");
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}
