using CampusLearn.DataModel.Models.Quizzes;

namespace CampusLearn.Services.Domain.Quizzes;

public class QuizService : IQuizService
{
    private readonly IQuizRepository _quizRepository;

    public QuizService(IQuizRepository quizRepository)
    {
        _quizRepository = quizRepository;
    }

    public async Task<Guid?> CreateQuiz(Guid userId, CreateQuizRequestModel model, CancellationToken token)
    {
        var result = await _quizRepository.CreateQuizAsync(userId, model, token);
        return result?.Body;
    }

    public async Task<Guid?> CreateQuizAttempt(Guid quizId, Guid userId, Guid assignedByUserId, CancellationToken token)
    {
        var result = await _quizRepository.CreateQuizAttemptAsync(quizId, userId, assignedByUserId, token);
        return result?.Body;
    }

    public async Task CreateQuizAttempt(Guid quizAttemptId, CompleteQuizAttemptRequestModel model, CancellationToken token)
    {
        await _quizRepository.CompleteQuizAttemptAsync(quizAttemptId, model, token);
    }

    public async Task<Guid?> CreateQuizQuestion(Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token)
    {
        var result = await _quizRepository.CreateQuizQuestionAsync(quizId, userId, model, token);
        return result?.Body;
    }

    public async Task<List<QuizViewModel>> GetActiveQuizzes(Guid userId, CancellationToken token)
    {
        return await _quizRepository.GetActiveQuizzesAsync(userId, token);
    }

    public async Task<QuizAttemptHistoryViewModel?> GetQuizAttemptHistory(Guid id, Guid userId, CancellationToken token)
    {
        return await _quizRepository.GetQuizAttemptHistoryAsync(id, userId, token);
    }

    public async Task<QuizDetailViewModel?> GetQuizDetails(Guid id, CancellationToken token)
    {
        return await _quizRepository.GetQuizDetailsAsync(id, token);
    }

    public async Task<List<QuizHistoryViewModel>> GetQuizzesAttemptHistory(Guid userId, CancellationToken token)
    {
        return await _quizRepository.GetQuizzesAttemptHistoryAsync(userId, token);
    }

    public async Task<List<QuizViewModel>> GetQuizzesByTopic(Guid topicId, CancellationToken token)
    {
        return await _quizRepository.GetQuizzesByTopicAsync(topicId, token);
    }

    public async Task UpdateQuiz(Guid id, Guid userId, CreateQuizRequestModel model, CancellationToken token)
    {
        await _quizRepository.UpdateQuizAsync(id, userId, model, token);
    }

    public async Task UpdateQuizQuestion(Guid id, Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token)
    {
        await _quizRepository.UpdateQuizQuestionAsync(id, quizId, userId, model, token);
    }
}