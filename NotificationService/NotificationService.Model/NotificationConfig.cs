using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Model
{
    [Table("NotificationConfigs", Schema = "notification")]
    public class NotificationConfig
    {
        public Guid Id { get; set; }
        public bool Messages { get; set; }
        public bool Connections { get; set; }
        public bool Posts { get; set; }

        public Guid ProfileId { get; set; }
        //public virtual Profile Profile { get; set; }
    }
}
