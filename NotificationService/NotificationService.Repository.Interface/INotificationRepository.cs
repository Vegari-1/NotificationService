using NotificationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Repository.Interface
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<Notification> Save(Notification notification);


    }
}
