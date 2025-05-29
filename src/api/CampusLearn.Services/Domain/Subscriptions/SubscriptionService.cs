namespace CampusLearn.Services.Domain.Subscriptions;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(ISubscriptionRepository subscriptionRepository)
    {
        _subscriptionRepository = subscriptionRepository;
    }

    public async Task<List<TutorSubscriptionViewModel>> GetAvailableTutors(Guid userId, CancellationToken token)
    {
        return await _subscriptionRepository.GetAvailableTutors(userId, token);
    }

    public async Task<List<TopicSubscriptionViewModel>> GetTopics(Guid userId, CancellationToken token)
    {
        return await _subscriptionRepository.GetTopics(userId, token);
    }

    public async Task<List<TutorSubscriptionViewModel>> GetTutors(Guid userId, CancellationToken token)
    {
        return await _subscriptionRepository.GetTutors(userId, token);
    }

    public async Task<Guid?> SubscribeToTopic(Guid userId, Guid topicId, CancellationToken token)
    {
        return await _subscriptionRepository.SubscribeToTopic(userId, topicId, token);
    }

    public Task<Guid?> SubscribeToTutor(Guid userId, Guid tutorId, CancellationToken token)
    {
        return _subscriptionRepository.SubscribeToTutor(userId, tutorId, token);
    }

    public async Task UnsubscribeFromTopic(Guid userId, Guid topicId, CancellationToken token)
    {
        await _subscriptionRepository.UnsubscribeFromTopic(userId, topicId, token);
    }

    public async Task UnsubscribeFromTutor(Guid userId, Guid tutorId, CancellationToken token)
    {
        await _subscriptionRepository.UnsubscribeFromTutor(userId, tutorId, token);
    }

    public async Task<List<Guid>> GetSubscribedUserIdentifiers(Guid? userId, Guid? tutorId, CancellationToken token)
    {
        return await _subscriptionRepository.GetSubscribedUserIdentifiers(userId, tutorId, token);
    }
}
