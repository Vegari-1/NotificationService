using BusService;
using BusService.Contracts;
using NotificationService.Model;
using NotificationService.Repository.Interface;
using NotificationService.Service.Interface;
using NotificationService.Service.Interface.Exceptions;
using NotificationService.Service.Interface.Sync;

namespace NotificationService.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly INotificationConfigRepository _notificationConfigRepository;
        private readonly IMessageSyncService _messageSyncService;

        public NotificationService(IEmailService emailService, 
            INotificationConfigRepository notificationConfigRepository,
            IMessageSyncService messageSyncService)
        {
            _emailService = emailService;
            _notificationConfigRepository = notificationConfigRepository;
            _messageSyncService = messageSyncService;
        }
        public void Send(Notification notification)
        {
            _emailService.SendEmail(notification);
        }

        public async Task<NotificationConfig> GetByProfileId(Guid profileId)
        {
            return await _notificationConfigRepository.GetByProfileIdAsync(profileId);
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
