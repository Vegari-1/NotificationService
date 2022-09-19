using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Dto;
using NotificationService.Model;
using NotificationService.Service.Interface;
using OpenTracing;
using Prometheus;
using System.ComponentModel.DataAnnotations;

namespace NotificationService
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        private readonly ITracer _tracer;

        Counter counter = Metrics.CreateCounter("notification_service_counter", "email counter");


        public NotificationController(INotificationService notificationService, IMapper mapper, ITracer tracer)
        {
            this._notificationService = notificationService; 
            this._mapper = mapper;
            this._tracer = tracer;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsByProfileId(
            [FromHeader(Name = "profile-id")][Required] Guid profileId)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("get notifications for profile");

            counter.Inc();

            NotificationConfig notification = await _notificationService.GetByProfileId(profileId);

            return Ok(_mapper.Map<NotificationConfigResponse>(notification));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotifications(
            [FromHeader(Name = "profile-id")][Required] Guid profileId,
            [FromBody] NotificationConfigRequest notificationRequest)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("update notifications");

            counter.Inc();

            NotificationConfig notification = await _notificationService.Update(profileId, _mapper.Map<NotificationConfig>(notificationRequest));

            return Ok(_mapper.Map<NotificationConfigResponse>(notification));
        }

    }
}
