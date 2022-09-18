using Microsoft.EntityFrameworkCore;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface.Sync;
using PostService.Repository;

namespace NotificationService.Repository.Sync
{
    public class ProfileRepository : Repository<Profile>, IProfileRepository
    {
        public ProfileRepository(AppDbContext context) : base(context) { }

        public Profile GetById(Guid id)
        {
            return _context.Profiles
                            .Where(x => x.Id == id)
                            .FirstOrDefault();
        }

        public async Task<Profile> GetByIdAsync(Guid id)
        {
            return await _context.Profiles
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
        }
    }
}
