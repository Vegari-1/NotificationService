namespace NotificationService.Dto
{
    public class NotificationConfigResponse
    {
        public Guid Id { get; set; }
        public bool Messages { get; set; }
        public bool Connections { get; set; }
        public bool Posts { get; set; }
    }
}
