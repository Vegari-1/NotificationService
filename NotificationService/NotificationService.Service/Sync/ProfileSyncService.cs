using BusService;
using BusService.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Model;
using NotificationService.Model.Sync;
using NotificationService.Repository.Interface;
using NotificationService.Repository.Interface.Sync;
using NotificationService.Service.Interface.Sync;

namespace NotificationService.Service.Sync
{
    public class ProfileSyncService : ConsumerBase<Profile, ProfileContract>, IProfileSyncService
    {
        private readonly IMessageBusService _messageBusService;
        private readonly IProfileRepository _profileRepository;
        private readonly INotificationConfigRepository _notificationConfigRepository;

        public ProfileSyncService(IMessageBusService messageBusService, IProfileRepository profileRepository,
            INotificationConfigRepository notificationConfigRepository, 
            ILogger<ProfileSyncService> logger) : base(logger)
        {
            _messageBusService = messageBusService;
            _profileRepository = profileRepository;
            _notificationConfigRepository = notificationConfigRepository;
        }

        public override Task PublishAsync(Profile entity, string action)
        {
            throw new NotImplementedException();
        }

        public override Task SynchronizeAsync(ProfileContract entity, string action)
        {
            if (action == Events.Created)
            {
                Profile profile = new Profile
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Surname = entity.Surname,
                    Username = entity.Username,
                    Email = entity.Email
                };
                profile = _profileRepository.Save(profile);
                NotificationConfig config = new NotificationConfig
                {
                    Connections = false,
                    Messages = false,
                    Posts = false,
                    ProfileId = profile.Id
                };
                _notificationConfigRepository.Save(config);
            }
            if (action == Events.Updated)
            {
                Profile dbProfile = _profileRepository.GetById(entity.Id);
                dbProfile.Name = entity.Name;
                dbProfile.Surname = entity.Surname;
                dbProfile.Username = entity.Username;
                dbProfile.Email = entity.Email;
                _profileRepository.SaveChanges();
            }
            return Task.CompletedTask;
        }
    }
}
