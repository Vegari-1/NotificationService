using NotificationService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace NotificationService.Service.Interface
{
    public interface INotificationService
    {
        public void Send(Model.Notification notification);
    }
}
