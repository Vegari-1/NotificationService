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
    public class MessageSyncService : ConsumerBase<MessageContract, MessageContract>, IMessageSyncService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IProfileRepository _profileRepository;
        private readonly INotificationConfigRepository _notificationConfigRepository;
        private readonly IEmailService _emailService;

        public MessageSyncService(IMessageBusService messageBusService, 
            IProfileRepository profileRepository, IEmailService emailService,
            INotificationConfigRepository notificationConfigRepository,
            ILogger<MessageSyncService> logger) : base(logger)
        {
            _messageBusService = messageBusService;
            _profileRepository = profileRepository;
            _notificationConfigRepository = notificationConfigRepository;
            _emailService = emailService;
        }

        public override Task PublishAsync(MessageContract entity, string action)
        {
            throw new NotImplementedException();
        }

        public override Task SynchronizeAsync(MessageContract entity, string action)
        {
            if (action == Events.Created)
            {
                Profile receiver = _profileRepository.GetById(entity.ReceiverId);
                if (receiver == null)
                    return Task.CompletedTask;

                NotificationConfig config = _notificationConfigRepository.GetByProfileId(receiver.Id);
                if (!config.Messages)
                    return Task.CompletedTask;

                Profile sender = _profileRepository.GetById(entity.SenderId);
                if (sender == null)
                    return Task.CompletedTask;

                Notification notification = new Notification
                {
                    Title = "Dislinkt - New Message",
                    Content = String.Format("You have a new message from {0} {1} (@{2})",
                                sender.Name, sender.Surname, sender.Username),
                    Recipent = receiver.Email
                };
                _emailService.SendEmail(notification);
            }
            return Task.CompletedTask;
        }

    }
}
