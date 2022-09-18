using NotificationService.Model.Sync;

namespace NotificationService.Repository.Interface.Sync
{
	public interface IConnectionRepository : IRepository<Connection>
	{
		Connection GetById(Guid id);
		Task<Connection> GetByIdAsync(Guid id);
    }
}
