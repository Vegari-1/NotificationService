using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Model
{
    public class Notification
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public string? Recipent { get; set; }

        public NotificationType Type { get; set; }
    }
}
