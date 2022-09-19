using Microsoft.EntityFrameworkCore;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface.Sync;

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

        public IEnumerable<Profile> GetConnectedProfilesForProfileId(Guid profileId)
        {
            // TODO: napisati ovo kao SQL upit
            List<Connection> connectedProfiles = _context.Connections
                                                .Where(x => x.Profile1 == profileId
                                                        || x.Profile2 == profileId)
                                                .ToList();
            IEnumerable<Guid> connectedProfileIds = connectedProfiles.Select(x => x.Profile1)
                                                    .Union(connectedProfiles.Select(x => x.Profile2));

            return _context.Profiles
                            .Where(x => x.Id != profileId)
                            .Where(x => connectedProfileIds.Contains(x.Id));
        }
    }
}
