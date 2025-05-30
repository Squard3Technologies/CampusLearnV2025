using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models.Enums;
using CampusLearn.DataModel.Models.Topic;
using CampusLearn.DataModel.ViewModels;
using CampusLearn.Services.Domain.Modules;
using CampusLearn.Services.Domain.Notifications;
using Microsoft.Extensions.Logging;
using Xunit;
using Assert = Xunit.Assert;

namespace CampusLearn.APITest;

public class ModuleServiceTest
{
    private readonly Mock<IModuleRepository> moduleRepositoryMock;
    private readonly Mock<INotificationService> notificationServiceMock;
    private readonly Mock<ILogger<ModuleService>> loggerMock;
    private readonly IModuleService moduleService;

    public ModuleServiceTest()
    {
        moduleRepositoryMock = new Mock<IModuleRepository>();
        notificationServiceMock = new Mock<INotificationService>();
        loggerMock = new Mock<ILogger<ModuleService>>();
        moduleService = new ModuleService(loggerMock.Object, moduleRepositoryMock.Object, notificationServiceMock.Object);
    }


    [Fact]
    public async Task CreateTopicAsync_WithValidData_ShouldSendNotificationAndReturnSuccess()
    {
        // Arrange
        var userId = Guid.Parse("E951C207-A23C-49E4-AF42-AFC4C501283E");
        var topicId = Guid.NewGuid();
        var model = new CreateTopicRequest()
        {
            Title = "Intro to databases",
            Description = "What is an RDBMS database?",
            ModuleId = Guid.Parse("DF35CFF4-D97F-47D1-8226-071BCA825CCD")
        };

        var token = CancellationToken.None;
        var expectedResposne = new GenericDbResponseViewModel<Guid?>
        {
            Status = true,
            StatusCode = 200,
            StatusMessage = "Success",
            Body = topicId
        };

        moduleRepositoryMock
            .Setup(r => r.AddTopicAsync(userId, model))
            .ReturnsAsync(expectedResposne);

        notificationServiceMock
            .Setup(n => n.SendTopicCreatedAsync(userId, topicId, NotificationTypes.Email, token))
            .Returns(Task.CompletedTask);

        // Act
        var addTopicResponse = await moduleService.AddTopicAsync(userId, model, token);

        // Assert
        Assert.True(addTopicResponse.Status);
        Assert.Equal(200, addTopicResponse.StatusCode);
        Assert.Equal("Success", addTopicResponse.StatusMessage);
        Assert.Equal(topicId, addTopicResponse.Body);

        notificationServiceMock.Verify(n =>
            n.SendTopicCreatedAsync(userId, topicId, NotificationTypes.Email, token),
            Times.Once);
    }    




}