using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Dto;
using NotificationService.Service.Interface;
using OpenTracing;
using Prometheus;

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

        [HttpPost("email")]
        public IActionResult SendEmail([FromBody] MailRequest request)
        {
            var actionName = ControllerContext.ActionDescriptor.DisplayName;
            using var scope = _tracer.BuildSpan(actionName).StartActive(true);
            scope.Span.Log("send email");

            counter.Inc();

            _notificationService.Send(_mapper.Map<Model.Notification>(request));
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
