using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Dto;
using NotificationService.Service.Interface;

namespace NotificationService
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;

        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            this._notificationService = notificationService; 
            this._mapper = mapper;
        }

        [HttpPost("email")]
        public async Task<Model.Notification> SendEmail([FromBody] MailRequest request)
        {
            return await _notificationService.Send(_mapper.Map<Model.Notification>(request));
        }

    }
}
