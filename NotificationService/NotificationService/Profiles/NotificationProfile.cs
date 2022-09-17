using AutoMapper;
using NotificationService.Dto;
using NotificationService.Model;

namespace NotificationService.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            // Source -> Target
            CreateMap<MailRequest, Model.Notification>();
            CreateMap<NotificationConfigRequest, NotificationConfig>();
            CreateMap<NotificationConfig, NotificationConfigResponse>();
        }
    }
}
