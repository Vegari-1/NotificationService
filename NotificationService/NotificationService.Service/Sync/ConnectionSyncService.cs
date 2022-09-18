using BusService;
using BusService.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface.Sync;
using NotificationService.Service.Interface.Sync;

namespace NotificationService.Service.Sync
{
    public class ConnectionSyncService : ConsumerBase<Connection, ConnectionContract>, IConnectionSyncService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IConnectionRepository _connectionRepository;

        public ConnectionSyncService(IMessageBusService messageBusService, IConnectionRepository connectionRepository,
            ILogger<ConnectionSyncService> logger) : base(logger)
        {
            _messageBusService = messageBusService;
            _connectionRepository = connectionRepository;
        }

        public override Task PublishAsync(Connection entity, string action)
        {
            throw new NotImplementedException();
        }

        public override Task SynchronizeAsync(ConnectionContract entity, string action)
        {
            if (action == Events.Created)
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
            }
            return Task.CompletedTask;
        }
    }
}
