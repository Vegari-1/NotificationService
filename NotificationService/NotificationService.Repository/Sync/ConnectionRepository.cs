using Microsoft.EntityFrameworkCore;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface.Sync;
using PostService.Repository;

namespace NotificationService.Repository.Sync
{
    public class ConnectionRepository : Repository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(AppDbContext context) : base(context) { }

        public Connection GetById(Guid id)
        {
            return _context.Connections
                            .Where(x => x.Id == id)
                            .FirstOrDefault();
        }

        public async Task<Connection> GetByIdAsync(Guid id)
        {
            return await _context.Connections
                                .Where(x => x.Id == id)
                                .FirstOrDefaultAsync();
        }
    }
}
