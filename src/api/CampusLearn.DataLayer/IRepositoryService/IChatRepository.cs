namespace CampusLearn.DataLayer.IRepositoryService;

public interface IChatRepository
{
    Task<List<ChatViewModel>> GetChatsByUser(Guid userId, CancellationToken token);

    Task<List<ChatMessageViewModel>> GetChatMessages(Guid sentByUserId, Guid sentToUserId, CancellationToken token);

    Task<Guid?> CreateChatMessage(Guid sentByUserId, Guid sentToUserId, CreateChatMessageRequestModel model, CancellationToken token);
}
