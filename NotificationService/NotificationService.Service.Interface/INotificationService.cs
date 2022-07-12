using NotificationService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace NotificationService.Service.Interface
{
    public interface INotificationService
    {
        public Task<Model.Notification> Send(Model.Notification notification);
    }
}
