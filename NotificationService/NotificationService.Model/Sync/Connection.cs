using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Model.Sync
{
    [Table("Connections", Schema = "notification")]
    public class Connection
    {
        public Guid Id { get; set; }
        public Guid Profile1 { get; set; }
        public Guid Profile2 { get; set; }
    }
}
