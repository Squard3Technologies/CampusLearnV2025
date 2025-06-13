namespace CampusLearn.Services.Domain.Notifications;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IAdminRepository _adminRepository;
    private readonly IEnquiryRepository _enquiryService;
    private readonly SMTPManager _smtpManager;
    private bool _emailerBusy = false;
    private bool _smsBusy = false;

    public NotificationService(
        INotificationRepository notificationRepository,
        ISubscriptionService subscriptionService,
        IAdminRepository adminRepository,
        IEnquiryRepository enquiryService,
        SMTPManager smtpManager)
    {
        _notificationRepository = notificationRepository;
        _subscriptionService = subscriptionService;
        _adminRepository = adminRepository;
        _enquiryService = enquiryService;
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
        var result = await SendMessageAsync(new SendMessageRequest()
        {
            SenderId = Constants.AdminUserIdentifier,
            RecieverId = userId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.AccountApproved, token);
        if (!result.Status)
            Console.WriteLine(result.StatusMessage);
    }

    public async Task SendAccountRejectedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.AccountRejected);
        var result = await SendMessageAsync(new SendMessageRequest()
        {
            SenderId = Constants.AdminUserIdentifier,
            RecieverId = userId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.AccountRejected, token);
        if (!result.Status)
            Console.WriteLine(result.StatusMessage);
    }

    public async Task SendAccountDeactivatedAsync(Guid userId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.AccountDeactivated);
        var result = await SendMessageAsync(new SendMessageRequest()
        {
            SenderId = Constants.AdminUserIdentifier,
            RecieverId = userId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.AccountDeactivated, token);
        if (!result.Status)
            Console.WriteLine(result.StatusMessage);
    }

    public async Task SendTopicCreatedAsync(Guid createdByUserId, Guid topicId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.TopicCreated);
        var subscribedUserIdentifiers = await _subscriptionService.GetSubscribedUserIdentifiers(createdByUserId, topicId, token);
        if (subscribedUserIdentifiers == null)
            return;

        foreach (var userIdentifier in subscribedUserIdentifiers)
        {
            var result = await SendMessageAsync(new SendMessageRequest()
            {
                SenderId = Constants.AdminUserIdentifier,
                RecieverId = userIdentifier,
                MessageContent = template
            }, notificationType, NotificationContentTypes.TopicCreated, token);
            if (!result.Status)
                Console.WriteLine(result.StatusMessage);
        }
    }

    public Task SendTopicQuizCreateAsync(Guid createdByUserId, Guid quizId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SendTopicQuizUpdatedAsync(Guid updatedByUserId, Guid quizId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SendTopicDiscussionCreatedAsync(Guid createdByUserId, Guid discussionId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SendTopicCommentCreatedAsync(Guid createdByUserId, Guid commentId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SendTopicLearningMaterialUploadedAsync(Guid createdByUserId, Guid learningMaterialId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task SendTopicQuizAssignedAsync(Guid createdByUserId, Guid quizAttemptId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public async Task SendEnquiryCreatedAsync(Guid createdByUserId, Guid enquiryId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.EnquiryCreated);
        var usersBody = await _adminRepository.GetUsersAsync();
        var users = (usersBody?.Body != null) ? (List<UserViewModel>)usersBody.Body : [];
        var tutors = users.Where(x => x.Role == UserRoles.Tutor);

        if (tutors == null)
            return;

        foreach (var tutor in tutors)
        {
            var result = await SendMessageAsync(new SendMessageRequest()
            {
                SenderId = Constants.AdminUserIdentifier,
                RecieverId = tutor.Id,
                MessageContent = template
            }, notificationType, NotificationContentTypes.EnquiryCreated, token);
            if (!result.Status)
                Console.WriteLine(result.StatusMessage);
        }
    }

    public async Task SendEnquiryResolvedAsync(Guid createdByUserId, Guid enquiryId, NotificationTypes notificationType, CancellationToken token)
    {
        var template = await _notificationRepository.GetEmailTemplate(NotificationContentTypes.EnquiryCreated);
        var enquiry = await _enquiryService.GetEnquiryAsync(enquiryId, token);

        if (enquiry == null)
            return;

        var result = await SendMessageAsync(new SendMessageRequest()
        {
            SenderId = Constants.AdminUserIdentifier,
            RecieverId = enquiry.CreatedByUserId,
            MessageContent = template
        }, notificationType, NotificationContentTypes.EnquiryResolved, token);
        if (!result.Status)
            Console.WriteLine(result.StatusMessage);
    }

    public Task SendChatMessageCreatedAsync(Guid createdByUserId, Guid messageId, NotificationTypes notificationType, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}