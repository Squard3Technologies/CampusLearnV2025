namespace CampusLearn.Services.Domain.Chats;

public class DiscussionService : IDiscussionService
{
    private readonly IDiscussionRepository _discussionRepository;

    public DiscussionService(IDiscussionRepository discussionRepository)
    {
        _discussionRepository = discussionRepository;
    }

    public async Task<Guid?> CreateComment(Guid discussionId, Guid userId, CreateDiscussionCommentRequestModel model, CancellationToken token)
    {
        return await _discussionRepository.CreateComment(discussionId, userId, model, token);
    }

    public async Task<Guid?> CreateDiscussion(Guid topicId, Guid userId, CreateDiscussionRequestModel model, CancellationToken token)
    {
        return await _discussionRepository.CreateDiscussion(topicId, userId, model, token);
    }

    public async Task<List<DiscussionCommentViewModel>> GetDiscussionComments(Guid discussionId, CancellationToken token)
    {
        return await _discussionRepository.GetDiscussionComments(discussionId, token);
    }

    public async Task<List<DiscussionViewModel>> GetDiscussionsByTopic(Guid topicId, CancellationToken token)
    {
        return await _discussionRepository.GetDiscussionsByTopic(topicId, token);
    }
}
