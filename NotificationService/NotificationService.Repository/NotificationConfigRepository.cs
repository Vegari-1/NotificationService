using Microsoft.EntityFrameworkCore;
using NotificationService.Model;
using NotificationService.Repository.Interface;
using PostService.Repository;

namespace NotificationService.Repository
{
    public class NotificationConfigRepository : Repository<NotificationConfig>, INotificationConfigRepository
    {
        public NotificationConfigRepository(AppDbContext context) : base(context) { }

        public async Task<NotificationConfig> GetById(Guid id)
        {
            return await _context.NotificationConfigs
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
        }

        public async Task<NotificationConfig> GetByProfileId(Guid profileId)
        {
            return await _context.NotificationConfigs
                                .Where(x => x.ProfileId == profileId)
                                .FirstOrDefaultAsync();
        }
    }
}
