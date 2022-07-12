using Microsoft.EntityFrameworkCore;
using NotificationService.Model;
using NotificationService.Repository;

namespace PostService.Repository
{
    public class AppDbContext : DbContext, IAppDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) { }

        public DbSet<Notification> Notifications { get; set; }
    }
}
