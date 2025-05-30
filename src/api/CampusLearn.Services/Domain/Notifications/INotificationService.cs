namespace CampusLearn.Services.Domain.Notifications;

public interface INotificationService
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, NotificationTypes notificationType, NotificationContentTypes notificationContentType, CancellationToken token);

    Task SendAccountApprovedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token);

    Task SendAccountRejectedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token);

    Task SendAccountDeactivatedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicCreatedAsync(Guid createdByUserId, Guid topicId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicQuizCreateAsync(Guid createdByUserId, Guid quizId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicQuizUpdatedAsync(Guid updatedByUserId, Guid quizId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicDiscussionCreatedAsync(Guid createdByUserId, Guid discussionId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicCommentCreatedAsync(Guid createdByUserId, Guid commentId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicLearningMaterialUploadedAsync(Guid createdByUserId, Guid learningMaterialId, NotificationTypes notificationType, CancellationToken token);

    Task SendTopicQuizAssignedAsync(Guid createdByUserId, Guid quizAttemptId, NotificationTypes notificationType, CancellationToken token);

    Task SendEnquiryCreatedAsync(Guid createdByUserId, Guid enquiryId, NotificationTypes notificationType, CancellationToken token);

    Task SendEnquiryResolvedAsync(Guid createdByUserId, Guid enquiryId, NotificationTypes notificationType, CancellationToken token);

    Task SendChatMessageCreatedAsync(Guid createdByUserId, Guid messageId, NotificationTypes notificationType, CancellationToken token);

    Task StartEmailingServiceAsync();

    bool EMAILServiceBusy();

    bool SMSServiceBusy();
}
