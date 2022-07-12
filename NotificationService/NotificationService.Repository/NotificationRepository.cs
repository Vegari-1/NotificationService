using NotificationService.Model;
using NotificationService.Repository.Interface;
using PostService.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Repository
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(AppDbContext context) : base(context) { }

        public async Task<Notification> Save(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            _context.SaveChanges();

            return notification;
        }
    }
}
