using CampusLearn.DataModel.Models.Messages;

namespace CampusLearn.Services.Domain.Notifications;

public interface INotificationService
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, NotificationTypes notificationType);

    Task StartEmailingServiceAsync();

    bool EMAILServiceBusy();

    bool SMSServiceBusy();
}
