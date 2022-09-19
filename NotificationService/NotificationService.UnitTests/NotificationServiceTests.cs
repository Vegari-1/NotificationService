using System;
using Moq;
using Xunit;
using NotificationService.Model;
using NotificationService.Service.Interface;
using NotificationService.Repository.Interface;
using NotificationService.Service.Interface.Exceptions;

namespace NotificationService.UnitTests;

public class NotificationServiceTests
{

    private static readonly Guid id = Guid.NewGuid();
    private static readonly Guid profileId = Guid.NewGuid();
    private static readonly bool connections = true;
    private static readonly bool  messages = true;
    private static readonly bool posts = true;
    private static readonly bool connectionsUpdate = false;
    private static readonly bool messagesUpdate = false;
    private static readonly bool postsUpdate = false;

    private static NotificationConfig notificationConfig;
    private static NotificationConfig notificationConfigUpdate;

    private static Mock<IEmailService> mockEmailService = new Mock<IEmailService>();
    private static Mock<INotificationConfigRepository> mockConfigRepository = new Mock<INotificationConfigRepository>();

    Service.NotificationService notificationService = new Service.NotificationService(mockEmailService.Object, mockConfigRepository.Object);

    private static void SetUp()
    {
        notificationConfig = new NotificationConfig()
        {
            Id = id,
            Connections = connections,
            Messages = messages,
            Posts = posts,
            ProfileId = profileId
        };

        notificationConfigUpdate = new NotificationConfig()
        {
            Id = id,
            Connections = connectionsUpdate,
            Messages = messagesUpdate,
            Posts = postsUpdate,
            ProfileId = profileId
        };
    }

    [Fact]
    public async void GetByProfileId_ProfileId_NotificationConfig()
    {
        SetUp();

        mockConfigRepository
            .Setup(service => service.GetByProfileIdAsync(profileId))
            .ReturnsAsync(notificationConfig);

        var response = await notificationService.GetByProfileId(profileId);

        Assert.Equal(notificationConfig.Id, response.Id);
        Assert.Equal(notificationConfig.Connections, response.Connections);
        Assert.Equal(notificationConfig.Messages, response.Messages);
        Assert.Equal(notificationConfig.Posts, response.Posts);
    }

    [Fact]
    public async void GetByProfileId_NonExistingProfileId_EntityNotFoundException()
    {
        SetUp();

        mockConfigRepository
            .Setup(service => service.GetByProfileIdAsync(profileId))
            .ReturnsAsync(notificationConfig);

        try
        {
            var response = await notificationService.GetByProfileId(Guid.Empty);
        }
        catch (Exception ex)
        {
            var thrownException = Assert.IsType<EntityNotFoundException>(ex);
        }
    }

    [Fact]
    public async void Update_CorrectData_NotificationConfig()
    {
        SetUp();

        mockConfigRepository
            .Setup(service => service.GetByIdAsync(id))
            .ReturnsAsync(notificationConfig);

        mockConfigRepository
            .Setup(service => service.SaveChangesAsync())
            .ReturnsAsync(1);

        var response = await notificationService.Update(profileId, notificationConfigUpdate);

        Assert.Equal(notificationConfigUpdate.Id, response.Id);
        Assert.Equal(notificationConfigUpdate.Connections, response.Connections);
        Assert.Equal(notificationConfigUpdate.Messages, response.Messages);
        Assert.Equal(notificationConfigUpdate.Posts, response.Posts);
    }

    [Fact]
    public async void Update_InvalidId_EntityNotFoundException()
    {
        SetUp();

        mockConfigRepository
            .Setup(service => service.GetByIdAsync(id))
            .ReturnsAsync(notificationConfig);

        mockConfigRepository
            .Setup(service => service.SaveChangesAsync())
            .ReturnsAsync(1);

        notificationConfigUpdate.Id = Guid.Empty;

        try
        {
            var response = await notificationService.Update(profileId, notificationConfigUpdate);
        }
        catch (Exception ex)
        {
            var thrownException = Assert.IsType<EntityNotFoundException>(ex);
        }
    }

    [Fact]
    public async void Update_InvalidProfileId_ForbiddenException()
    {
        SetUp();

        mockConfigRepository
            .Setup(service => service.GetByIdAsync(id))
            .ReturnsAsync(notificationConfig);

        mockConfigRepository
            .Setup(service => service.SaveChangesAsync())
            .ReturnsAsync(1);

        try
        {
            var response = await notificationService.Update(Guid.Empty, notificationConfigUpdate);
        }
        catch (Exception ex)
        {
            var thrownException = Assert.IsType<ForbiddenException>(ex);
        }
    }

}
