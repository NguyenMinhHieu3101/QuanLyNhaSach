using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;

namespace UngDungQuanLyNhaSach.Model
{
    public class Mailer
    {
        private static readonly string _from = "smtpserverlhn@gmail.com";
        private static readonly string _pass = "txedtoecqoemqqqv";

        public string Send(string sendto, string subject, string content)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_from);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(_from, _pass);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
    }
}







