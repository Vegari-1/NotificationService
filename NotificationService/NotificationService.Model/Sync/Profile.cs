using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationService.Model.Sync
{
    [Table("Profiles", Schema = "notification")]
    public class Profile
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }
}
