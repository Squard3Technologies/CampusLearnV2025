namespace CampusLearn.DataLayer.IRepositoryService;

public interface ISubscriptionRepository
{
    Task<List<TutorSubscriptionViewModel>> GetTutors(Guid userId, CancellationToken token);

    Task<Guid?> SubscribeToTutor(Guid userId, Guid tutorId, CancellationToken token);

    Task UnsubscribeFromTutor(Guid userId, Guid tutorId, CancellationToken token);

    Task<List<TutorSubscriptionViewModel>> GetAvailableTutors(Guid userId, CancellationToken token);

    Task<List<TopicSubscriptionViewModel>> GetTopics(Guid userId, CancellationToken token);

    Task<Guid?> SubscribeToTopic(Guid userId, Guid topicId, CancellationToken token);

    Task UnsubscribeFromTopic(Guid userId, Guid topicId, CancellationToken token);

    Task<List<Guid>> GetSubscribedUserIdentifiers(Guid? userId, Guid? tutorId, CancellationToken token);
}
