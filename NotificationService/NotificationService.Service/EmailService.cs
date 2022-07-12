using NotificationService.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Mail;
using MimeKit;
using MimeKit.Text;
using NotificationService.Repository.Interface;

namespace NotificationService.Service
{
    public class EmailService : IEmailService
    {

        public void SendEmail(Model.Notification notification)
        {
            MailMessage mail = CreateMail(notification);
            SmtpClient client = CreateSmtpClient();

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MailMessage CreateMail(Model.Notification notification)
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("dislinktapp@gmail.com");
            mail.To.Add(notification.Recipent);
            mail.To.Add("nikolakabasaj6@gmail.com");
            mail.Subject = notification.Title;
            mail.IsBodyHtml = true;
            mail.Body = notification.Content;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = false;
            return mail;
        }

        private SmtpClient CreateSmtpClient() {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("dislinktapp@gmail.com", "aknouvoipsxctaep");
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
    }
}
