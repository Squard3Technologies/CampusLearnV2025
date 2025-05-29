using CampusLearn.DataModel.Models.Messages;

namespace CampusLearn.Services.Domain.Notifications;

public interface INotificationService
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, NotificationTypes notificationType, NotificationContentTypes notificationContentType);

    Task StartEmailingServiceAsync();

    bool EMAILServiceBusy();

    bool SMSServiceBusy();
}