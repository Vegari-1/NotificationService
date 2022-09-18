using BusService;
using BusService.Contracts;

namespace NotificationService.Service.Interface.Sync
{
    public interface IMessageSyncService : ISyncService<MessageContract, MessageContract>
    {
    }
}
