namespace CampusLearn.DataLayer.IRepositoryService;

public interface IDiscussionRepository
{
    Task<List<DiscussionViewModel>> GetDiscussionsByTopic(Guid topicId, CancellationToken token);

    Task<List<DiscussionCommentViewModel>> GetDiscussionComments(Guid discussionId, CancellationToken token);

    Task<Guid?> CreateDiscussion(Guid topicId, Guid userId, CreateDiscussionRequestModel model, CancellationToken token);

    Task<Guid?> CreateComment(Guid discussionId, Guid userId, CreateDiscussionCommentRequestModel model, CancellationToken token);
}
