using NotificationService.Model;
using NotificationService.Repository.Interface;
using NotificationService.Service.Interface;
using NotificationService.Service.Interface.Exceptions;

namespace NotificationService.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly INotificationConfigRepository _notificationConfigRepository;

        public NotificationService(IEmailService emailService, 
            INotificationConfigRepository notificationConfigRepository)
        {
            _emailService = emailService;
            _notificationConfigRepository = notificationConfigRepository;
        }
        public void Send(Notification notification)
        {
            _emailService.SendEmail(notification);
        }

        public async Task<NotificationConfig> GetByProfileId(Guid profileId)
        {
            NotificationConfig notificationConfig = await _notificationConfigRepository.GetByProfileIdAsync(profileId);
            if (notificationConfig == null)
                throw new EntityNotFoundException(typeof(NotificationConfig), "profile id");

            return notificationConfig;
        }

        public async Task<NotificationConfig> Update(Guid profileId, NotificationConfig notificationConfig)
        {
            NotificationConfig dbConfig = await _notificationConfigRepository.GetByIdAsync(notificationConfig.Id);

            if (dbConfig == null)
                throw new EntityNotFoundException(typeof(NotificationConfig), "id");
            if (!profileId.Equals(dbConfig.ProfileId))
                throw new ForbiddenException();

            dbConfig.Messages = notificationConfig.Messages;
            dbConfig.Connections = notificationConfig.Connections;
            dbConfig.Posts = notificationConfig.Posts;

            await _notificationConfigRepository.SaveChangesAsync();

            return dbConfig;
        }
    }
}
