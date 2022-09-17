using NotificationService.Service.Interface;
using System.Text;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace NotificationService.Service
{
    public class EmailService : IEmailService
    {

        private readonly IConfiguration Configuration;

        public EmailService(IConfiguration configuration)   
        {
            Configuration = configuration;
        }

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
            var mailFrom = Configuration["Mail:From"];
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailFrom);
            mail.To.Add(notification.Recipent);
            mail.Subject = notification.Title;
            mail.IsBodyHtml = true;
            mail.Body = notification.Content;
            mail.BodyEncoding = Encoding.UTF8;
            mail.IsBodyHtml = false;
            return mail;
        }

        private SmtpClient CreateSmtpClient() {
            var mailFrom = Configuration["Mail:From"];
            var mailKey = Configuration["Mail:Key"];
            var smtpHost = Configuration["Mail:Smtp:Host"];
            var smtpPort = Int32.Parse(Configuration["Mail:Smtp:Port"]);

            SmtpClient client = new SmtpClient(smtpHost, smtpPort);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mailFrom, mailKey);
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            return client;
        }
    }
}
