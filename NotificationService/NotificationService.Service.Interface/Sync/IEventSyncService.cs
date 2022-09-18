using BusService;
using BusService.Contracts;

namespace NotificationService.Service.Interface.Sync
{
    public interface IEventSyncService : ISyncService<EventContract, EventContract>
    {
    }
}
