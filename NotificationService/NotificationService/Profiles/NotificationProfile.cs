using AutoMapper;
using NotificationService.Dto;

namespace NotificationService.Profiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            // Source -> Target
            CreateMap<MailRequest, Model.Notification>();
        }
    }
}
