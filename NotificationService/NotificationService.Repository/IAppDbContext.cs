using Microsoft.EntityFrameworkCore;
using NotificationService.Model;

namespace NotificationService.Repository
{
    public interface IAppDbContext
    {
        DbSet<Notification> Notifications { get; set; }
    }
}
