using BusService;
using BusService.Routing;
using NotificationService.Service.Interface.Sync;
using Polly;

namespace NotificationService.Messaging
{
    public class NotificationMessageBusService : MessageBusHostedService
    {
        public NotificationMessageBusService(IMessageBusService serviceBus, IServiceScopeFactory serviceScopeFactory) : base(serviceBus, serviceScopeFactory)
        {
        }

        protected override void ConfigureSubscribers()
        {
            var policy = BuildPolicy();
            Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Profile), typeof(IProfileSyncService)));
            Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Connection), typeof(IConnectionSyncService)));
            // todo: change topic route
            Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Connection), typeof(IMessageSyncService)));
            Subscribers.Add(new MessageBusSubscriber(policy, SubjectBuilder.Build(Topics.Connection), typeof(IPostSyncService)));
        }

        private Policy BuildPolicy()
        {
            return Policy
                    .Handle<Exception>()
                    .WaitAndRetry(5, _ => TimeSpan.FromSeconds(5), (exception, _, _, _) =>
                    { });
        }
    }
}
