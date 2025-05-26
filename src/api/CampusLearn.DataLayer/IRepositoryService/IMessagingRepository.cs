namespace CampusLearn.DataLayer.IRepositoryService;

public interface IMessagingRepository
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, string messageType);

    Task<GenericDbResponseViewModel> GetAllPendingMessagesAsync(string messageType, int batchSize = 100);

    Task UpdateMessageStatusAsync(MessageViewModel model, int statusCode, string statusMessage);
}