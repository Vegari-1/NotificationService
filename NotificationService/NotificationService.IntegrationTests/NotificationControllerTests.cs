using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using NotificationService;
using NotificationService.Dto;
using NotificationService.Model;
using NotificationService.Repository;
using System.Text;

namespace AuthService.IntegrationTests
{
    public class NotificationControllerTests : IClassFixture<IntegrationWebApplicationFactory<Program, AppDbContext>>
    {
        private readonly IntegrationWebApplicationFactory<Program, AppDbContext> _factory;
        private readonly HttpClient _client;

        public NotificationControllerTests(IntegrationWebApplicationFactory<Program, AppDbContext> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        private static readonly string schemaName = "notification";
        private static readonly string tableName = "NotificationConfigs";
        private static readonly Guid id = Guid.NewGuid();
        private static readonly Guid profileId = Guid.NewGuid();
        private static readonly bool connections = true;
        private static readonly bool messages = true;
        private static readonly bool posts = true;
        private static readonly bool connectionsUpdate = false;
        private static readonly bool messagesUpdate = false;
        private static readonly bool postsUpdate = false;

        [Fact]
        public async Task GetNotificationsByProfileId_ValidProfileId_NotificationConfigResponse()
        {
            // Given
            NotificationConfig config = new NotificationConfig()
            {
                Id = id,
                Messages = messages,
                Connections = connections,
                Posts = posts,
                ProfileId = profileId
            };
            _factory.Insert(schemaName, tableName, config);

            // When
            using (var request = new HttpRequestMessage(HttpMethod.Get, "api/notification"))
            {
                request.Headers.Add("profile-id", profileId.ToString());
                var response = await _client.SendAsync(request);

                // Then
                response.EnsureSuccessStatusCode();
                var responseContentString = await response.Content.ReadAsStringAsync();
                var responseContentObject = JsonConvert.DeserializeObject<NotificationConfigResponse>(responseContentString);
                Assert.NotNull(responseContentObject);
                Assert.Equal(id, responseContentObject.Id);
                Assert.Equal(connections, responseContentObject.Connections);
                Assert.Equal(messages, responseContentObject.Messages);
                Assert.Equal(posts, responseContentObject.Posts);
            }

            // Rollback
            _factory.DeleteById(schemaName, tableName, id);
        }

        [Fact]
        public async Task UpdateNotifications_ValidData_NotificationConfigResponse()
        {
            // Given
            NotificationConfig config = new NotificationConfig()
            {
                Id = id,
                Messages = messages,
                Connections = connections,
                Posts = posts,
                ProfileId = profileId
            };
            _factory.Insert(schemaName, tableName, config);

            NotificationConfigRequest configRequest = new NotificationConfigRequest()
            {
                Id = id,
                Connections = connectionsUpdate,
                Messages = messagesUpdate,
                Posts = postsUpdate,
                ProfileId = profileId
            };
            var requestContent = new StringContent(JsonConvert.SerializeObject(configRequest), Encoding.UTF8, "application/json");

            // When
            using (var request = new HttpRequestMessage(HttpMethod.Put, "api/notification"))
            {
                request.Headers.Add("profile-id", profileId.ToString());
                request.Content = requestContent;
                var response = await _client.SendAsync(request);

                // Then
                response.EnsureSuccessStatusCode();
                var responseContentString = await response.Content.ReadAsStringAsync();
                var responseContentObject = JsonConvert.DeserializeObject<NotificationConfigResponse>(responseContentString);
                Assert.NotNull(responseContentObject);
                Assert.Equal(id, responseContentObject.Id);
                Assert.Equal(connectionsUpdate, responseContentObject.Connections);
                Assert.Equal(messagesUpdate, responseContentObject.Messages);
                Assert.Equal(postsUpdate, responseContentObject.Posts);
                Assert.Equal(1L, _factory.CountTableRows(schemaName, tableName));
            }

            // Rollback
            _factory.DeleteById(schemaName, tableName, id);
        }

    }
}
