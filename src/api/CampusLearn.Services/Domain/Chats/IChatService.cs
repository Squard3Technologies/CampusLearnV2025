namespace CampusLearn.Services.Domain.Chats;

public interface IChatService
{
    Task<List<ChatViewModel>> GetChatsByUser(Guid userId, CancellationToken token);

    Task<List<ChatMessageViewModel>> GetChatMessages(Guid sentByUserId, Guid sentToUserId, CancellationToken token);

    Task<Guid?> CreateChatMessage(Guid sentByUserId, Guid sentToUserId, CreateChatMessageRequestModel model, CancellationToken token);
}
