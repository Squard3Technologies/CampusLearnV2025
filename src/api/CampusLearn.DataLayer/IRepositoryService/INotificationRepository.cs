namespace CampusLearn.DataLayer.IRepositoryService;

public interface INotificationRepository
{
    Task<GenericDbResponseViewModel> CreateNotificationAsync(SendMessageRequest model, NotificationTypes notificationType);

    Task<GenericDbResponseViewModel> GetAllPendingNotificationAsync(NotificationTypes notificationType, int batchSize = 100);

    Task UpdateNotificationStatusAsync(NotificationViewModel model, int statusCode, string statusMessage);
}
