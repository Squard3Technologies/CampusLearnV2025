namespace CampusLearn.Services.Domain.Chats;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;

    public ChatService(IChatRepository chatRepository)
    {
        _chatRepository = chatRepository;
    }

    public async Task<Guid?> CreateChatMessage(Guid sentByUserId, Guid sentToUserId, CreateChatMessageRequestModel model, CancellationToken token)
    {
        return await _chatRepository.CreateChatMessage(sentByUserId, sentToUserId, model, token);
    }

    public async Task<List<ChatMessageViewModel>> GetChatMessages(Guid sentByUserId, Guid sentToUserId, CancellationToken token)
    {
        return await _chatRepository.GetChatMessages(sentByUserId, sentToUserId, token);
    }

    public async Task<List<ChatViewModel>> GetChatsByUser(Guid userId, CancellationToken token)
    {
        return await _chatRepository.GetChatsByUser(userId, token);
    }
}
