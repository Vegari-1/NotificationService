using BusService;
using BusService.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Model;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface;
using NotificationService.Repository.Interface.Sync;
using NotificationService.Service.Interface;
using NotificationService.Service.Interface.Sync;

namespace NotificationService.Service.Sync
{
    public class ConnectionSyncService : ConsumerBase<Connection, ConnectionContract>, IConnectionSyncService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IConnectionRepository _connectionRepository;
        private readonly IProfileRepository _profileRepository;
        private readonly INotificationConfigRepository _notificationConfigRepository;
        private readonly IEmailService _emailService;

        public ConnectionSyncService(IMessageBusService messageBusService, IConnectionRepository connectionRepository,
            IProfileRepository profileRepository, INotificationConfigRepository notificationConfigRepository, 
            IEmailService emailService, 
            ILogger<ConnectionSyncService> logger) : base(logger)
        {
            _messageBusService = messageBusService;
            _connectionRepository = connectionRepository;
            _profileRepository = profileRepository;
            _notificationConfigRepository = notificationConfigRepository;
            _emailService = emailService;
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

                SendEmail(entity);
            }
            if (action == Events.Deleted)
            {
                Connection dbConnection = _connectionRepository.GetById(entity.Id);
                _connectionRepository.Delete(dbConnection);
            }
            return Task.CompletedTask;
        }

        private void SendEmail(ConnectionContract entity)
        {
            Profile receiver = _profileRepository.GetById(entity.Profile2);
            if (receiver == null)
                return;

            NotificationConfig config = _notificationConfigRepository.GetByProfileId(receiver.Id);
            if (!config.Connections)
                return;

            Profile sender = _profileRepository.GetById(entity.Profile1);
            if (sender == null)
                return;

            Notification notification = new Notification
            {
                Title = "Dislinkt - New Connection",
                Content = String.Format("You have a new connection from {0} {1} (@{2})",
                sender.Name, sender.Surname, sender.Username),
                Recipent = receiver.Email
            };
            _emailService.SendEmail(notification);
        }
    }
}
