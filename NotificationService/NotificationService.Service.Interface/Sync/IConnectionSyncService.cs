using BusService;
using BusService.Contracts;
using NotificationService.Model.Sync;

namespace NotificationService.Service.Interface.Sync
{
    public interface IConnectionSyncService : ISyncService<Connection, ConnectionContract>
    {
    }
}
