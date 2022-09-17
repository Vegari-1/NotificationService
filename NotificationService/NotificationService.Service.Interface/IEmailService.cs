namespace NotificationService.Service.Interface
{
    public interface IEmailService
    {
        void SendEmail(Model.Notification notification);
    }
}
