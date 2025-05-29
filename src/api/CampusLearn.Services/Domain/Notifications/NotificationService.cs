namespace CampusLearn.Services.Domain.Notifications;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ISubscriptionService _subscriptionService;
    private readonly SMTPManager _smtpManager;
    private bool _emailerBusy = false;
    private bool _smsBusy = false;

    public NotificationService(
        INotificationRepository notificationRepository,
        ISubscriptionService subscriptionService,
        SMTPManager smtpManager)
    {
        _notificationRepository = notificationRepository;
        _subscriptionService = subscriptionService;
        _smtpManager = smtpManager;
    }

    public bool EMAILServiceBusy() => _emailerBusy;

    public bool SMSServiceBusy() => _smsBusy;

    public async Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, NotificationTypes notificationType, NotificationContentTypes notificationContentType, CancellationToken token)
    {
        return await _notificationRepository.CreateNotificationAsync(model, notificationType);
    }

    public async Task StartEmailingServiceAsync()
    {
        _emailerBusy = true;
        try
        {
            var getMessageResponse = await _notificationRepository.GetAllPendingNotificationAsync(NotificationTypes.Email, 200);
            if (getMessageResponse.Status)
            {
                var messages = (List<NotificationViewModel>)getMessageResponse.Body;
                foreach (var message in messages)
                {
                    var sendMessageResponse = await _smtpManager.SendEmailAsync(message);
                    await _notificationRepository.UpdateNotificationStatusAsync(message, sendMessageResponse.StatusCode, sendMessageResponse.StatusMessage);
                }
            }
        }
        catch (Exception ex)
        {
        }
        _emailerBusy = false;
    }

    public async Task SendAccountApprovedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.AccountApproved);
        await SendMessageAsync(new SendMessageRequest()
        {
            RecieverId = userId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.AccountApproved, token);
    }

    public async Task SendAccountRejectedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.AccountRejected);
        await SendMessageAsync(new SendMessageRequest()
        {
            RecieverId = userId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.AccountRejected, token);
    }

    public async Task SendAccountDeactivatedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.AccountDeactivated);
        await SendMessageAsync(new SendMessageRequest()
        {
            RecieverId = userId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.AccountDeactivated, token);
    }

    public async Task SendTopicCreatedAsync(Guid createdByUserId, Guid topicId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.TopicCreated);
        var subscribedUserIdentifiers = await _subscriptionService.GetSubscribedUserIdentifiers(createdByUserId, topicId, token);
        if (subscribedUserIdentifiers == null)
            return;

        foreach (var userIdentifier in subscribedUserIdentifiers)
        {
            await SendMessageAsync(new SendMessageRequest()
            {
                RecieverId = userIdentifier,
                MessageContent = template
            }, notificationType, NotificationContentTypes.TopicCreated, token);
        }
    }

    public Task SendTopicQuizCreateAsync(Guid createdByUserId, Guid topicId, Guid quizId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SendTopicQuizUpdatedAsync(Guid updatedByUserId, Guid topicId, Guid quizId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
