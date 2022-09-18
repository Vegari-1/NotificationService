using BusService;
using BusService.Contracts;

namespace NotificationService.Service.Interface.Sync
{
    public interface IPostSyncService : ISyncService<PostContract, PostContract>
    {
    }
}
