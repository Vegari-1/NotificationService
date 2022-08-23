using NotificationService.Service.Interface;
using NotificationService.Model;


namespace NotificationService.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;

        public NotificationService(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public void Send(Model.Notification notification)
        {
            _emailService.SendEmail(notification);
        }

    }
}
