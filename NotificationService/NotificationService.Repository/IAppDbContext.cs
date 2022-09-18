using Microsoft.EntityFrameworkCore;
using NotificationService.Model;
using NotificationService.Model.Sync;

namespace NotificationService.Repository
{
    public interface IAppDbContext
    {
        DbSet<NotificationConfig> NotificationConfigs { get; set; }
        DbSet<Profile> Profiles { get; set; }
        DbSet<Connection> Connections { get; set; }
    }
}
