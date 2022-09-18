using NotificationService.Model;

namespace NotificationService.Repository.Interface
{
	public interface INotificationConfigRepository : IRepository<NotificationConfig>
	{
		NotificationConfig GetById(Guid id);
		Task<NotificationConfig> GetByIdAsync(Guid id);
		NotificationConfig GetByProfileId(Guid profileId);
		Task<NotificationConfig> GetByProfileIdAsync(Guid profileId);
    }
}
