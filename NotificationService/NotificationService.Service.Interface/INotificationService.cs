using NotificationService.Model;

namespace NotificationService.Service.Interface
{
    public interface INotificationService
    {
        public void Send(Notification notification);
        public Task<NotificationConfig> Update(Guid profileId, NotificationConfig notificationConfig);
        Task<NotificationConfig> GetByProfileId(Guid profileId);
    }
}
