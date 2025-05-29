using CampusLearn.DataModel.Models.Messages;
using CampusLearn.Services.Domain.Utils;

namespace CampusLearn.Services.Domain.Notifications;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly SMTPManager _smtpManager;
    private bool _emailerBusy = false;
    private bool _smsBusy = false;

    public NotificationService(INotificationRepository notificationRepository, SMTPManager smtpManager)
    {
        _notificationRepository = notificationRepository;
        _smtpManager = smtpManager;
    }

    public bool EMAILServiceBusy() => _emailerBusy;

    public bool SMSServiceBusy() => _smsBusy;

    public async Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, NotificationTypes notificationType)
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
}
