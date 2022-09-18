using Microsoft.EntityFrameworkCore;
using NotificationService.Model;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface;
using PostService.Repository;

namespace NotificationService.Repository
{
    public class NotificationConfigRepository : Repository<NotificationConfig>, INotificationConfigRepository
    {
        public NotificationConfigRepository(AppDbContext context) : base(context) { }

        public NotificationConfig GetById(Guid id)
        {
            return _context.NotificationConfigs
                        .Where(x => x.Id == id)
                        .FirstOrDefault();
        }

        public async Task<NotificationConfig> GetByIdAsync(Guid id)
        {
            return await _context.NotificationConfigs
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
        }

        public NotificationConfig GetByProfileId(Guid profileId)
        {
            return _context.NotificationConfigs
                        .Where(x => x.ProfileId == profileId)
                        .FirstOrDefault();
        }

        public async Task<NotificationConfig> GetByProfileIdAsync(Guid profileId)
        {
            return await _context.NotificationConfigs
                                .Where(x => x.ProfileId == profileId)
                                .FirstOrDefaultAsync();
        }

        public IEnumerable<NotificationConfig> GetByProfileIdList(IEnumerable<Guid> profileIds)
        {
            return _context.NotificationConfigs
                    .Where(x => profileIds.Contains(x.ProfileId))
                    .ToList();
        }

    }
}
