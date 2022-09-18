using BusService;
using BusService.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Repository.Interface.Sync;
using NotificationService.Service.Interface.Sync;

namespace NotificationService.Service.Sync
{
    public class PostSyncService : ConsumerBase<PostContract, PostContract>, IPostSyncService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IConnectionRepository _connectionRepository;

        public PostSyncService(IMessageBusService messageBusService, IConnectionRepository connectionRepository,
            ILogger<PostSyncService> logger) : base(logger)
        {
            _messageBusService = messageBusService;
            _connectionRepository = connectionRepository;
        }

        public override Task PublishAsync(PostContract entity, string action)
        {
            throw new NotImplementedException();
        }

        public override Task SynchronizeAsync(PostContract entity, string action)
        {
            /*if (action == Events.Created)
            {
                Connection connection = new Connection
                {
                    Id = entity.Id,
                    Profile1 = entity.Profile1,
                    Profile2 = entity.Profile2
                };
                _connectionRepository.Save(connection);
            }
            if (action == Events.Deleted)
            {
                Connection dbConnection = _connectionRepository.GetById(entity.Id);
                _connectionRepository.Delete(dbConnection);
            }*/
            return Task.CompletedTask;
        }
    }
}
