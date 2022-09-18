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
    public class PostSyncService : ConsumerBase<PostContract, PostContract>, IPostSyncService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IProfileRepository _profileRepository;
        private readonly INotificationConfigRepository _notificationConfigRepository;
        private readonly IEmailService _emailService;

        public PostSyncService(IMessageBusService messageBusService, 
            IProfileRepository profileRepository, IEmailService emailService,
            INotificationConfigRepository notificationConfigRepository, 
            ILogger<PostSyncService> logger) : base(logger)
        {
            _messageBusService = messageBusService;
            _profileRepository = profileRepository;
            _notificationConfigRepository = notificationConfigRepository;
            _emailService = emailService;
        }

        public override Task PublishAsync(PostContract entity, string action)
        {
            throw new NotImplementedException();
        }

        public override Task SynchronizeAsync(PostContract entity, string action)
        {
            Profile publisher = _profileRepository.GetById(entity.PublisherId);
            if (publisher == null)
                return Task.CompletedTask;

            List<Profile> connectedProfiles = _profileRepository.GetConnectedProfilesForProfileId(entity.PublisherId).ToList();
            
            List<NotificationConfig> configs = _notificationConfigRepository.GetByProfileIdList(
                                                        connectedProfiles.ToList().Select(x => x.Id)).ToList();
            Profile receiver;
            configs.ForEach(config => 
                {
                    if (config.Posts)
                    {
                        receiver = connectedProfiles.First(x => x.Id == config.ProfileId);
                        SendEmail(publisher, receiver);
                    }
                 });
            return Task.CompletedTask;
        }

        private void SendEmail(Profile publisher, Profile receiver)
        {
            Notification notification = new Notification
            {
                Title = "Dislinkt - New Post",
                Content = String.Format("{0} {1} (@{2}) has published a new post",
                publisher.Name, publisher.Surname, publisher.Username),
                Recipent = receiver.Email
            };
            _emailService.SendEmail(notification);
        }
    }
}
