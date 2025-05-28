using CampusLearn.DataModel.Models.Quizzes;

namespace CampusLearn.Services.Domain.Quizzes;

public interface IQuizService
{
    Task<List<QuizViewModel>> GetQuizzesByTopic(Guid topicId, CancellationToken token);

    Task<QuizDetailViewModel?> GetQuizDetails(Guid id, CancellationToken token);

    Task<Guid?> CreateQuiz(Guid userId, CreateQuizRequestModel model, CancellationToken token);

    Task UpdateQuiz(Guid id, Guid userId, CreateQuizRequestModel model, CancellationToken token);

    Task<Guid?> CreateQuizQuestion(Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token);

    Task UpdateQuizQuestion(Guid id, Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token);

    Task<List<QuizViewModel>> GetActiveQuizzes(Guid userId, CancellationToken token);

    Task<Guid?> CreateQuizAttempt(Guid quizId, Guid userId, CreateQuizAttemptRequestModel model, CancellationToken token);

    Task<List<QuizHistoryViewModel>> GetQuizzesAttemptHistory(Guid userId, CancellationToken token);

    Task<QuizAttemptHistoryViewModel?> GetQuizAttemptHistory(Guid id, Guid userId, CancellationToken token);
}
