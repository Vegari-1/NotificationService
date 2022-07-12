using NotificationService.Service.Interface;
using NotificationService.Repository.Interface;
using NotificationService.Model;


namespace NotificationService.Service
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;

        public NotificationService(INotificationRepository notificationRepository, IEmailService emailService)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
        }

        public Task<Model.Notification> Send(Model.Notification notification)
        {
            _emailService.SendEmail(notification);
            return _notificationRepository.Save(notification);
        }

    }
}
