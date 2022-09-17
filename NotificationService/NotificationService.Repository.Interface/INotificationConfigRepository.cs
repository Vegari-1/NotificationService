using NotificationService.Model;

namespace NotificationService.Repository.Interface
{
	public interface INotificationConfigRepository : IRepository<NotificationConfig>
	{
		Task<NotificationConfig> GetById(Guid id);
        Task<NotificationConfig> GetByProfileId(Guid profileId);
    }
}
