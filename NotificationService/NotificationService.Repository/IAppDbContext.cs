using Microsoft.EntityFrameworkCore;
using NotificationService.Model;

namespace NotificationService.Repository
{
    public interface IAppDbContext
    {
        DbSet<NotificationConfig> NotificationConfigs { get; set; }
    }
}
