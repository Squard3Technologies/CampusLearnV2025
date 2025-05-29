namespace CampusLearn.Services.Domain.Notifications;

public interface INotificationService
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, NotificationTypes notificationType, NotificationContentTypes notificationContentType, CancellationToken token);

    Task SendAccountApprovedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token);

    Task SendAccountRejectedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token);

    Task SendAccountDeactivatedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicCreatedAsync(Guid createdByUserId, Guid topicId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicQuizCreateAsync(Guid createdByUserId, Guid topicId, Guid quizId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicQuizUpdatedAsync(Guid updatedByUserId, Guid topicId, Guid quizId, NotificationTypes notificationType, CancellationToken token);

    Task StartEmailingServiceAsync();

    bool EMAILServiceBusy();

    bool SMSServiceBusy();
}
