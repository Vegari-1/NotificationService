using NotificationService.Model.Sync;

namespace NotificationService.Repository.Interface.Sync
{
	public interface IProfileRepository : IRepository<Profile>
	{
		Profile GetById(Guid id);
		Task<Profile> GetByIdAsync(Guid id);
		public IEnumerable<Profile> GetConnectedProfilesForProfileId(Guid profileId);

	}
}
