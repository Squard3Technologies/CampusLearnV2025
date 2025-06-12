using CampusLearn.DataModel.Models.Quizzes;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IQuizRepository
{
    Task<List<QuizViewModel>> GetQuizzesByTopicAsync(Guid topicId, CancellationToken token);

    Task<QuizDetailViewModel?> GetQuizDetailsAsync(Guid id, CancellationToken token);

    Task<GenericDbResponseViewModel<Guid?>> CreateQuizAsync(Guid userId, CreateQuizRequestModel model, CancellationToken token);

    Task<GenericDbResponseViewModel<Guid?>> UpdateQuizAsync(Guid id, Guid userId, CreateQuizRequestModel model, CancellationToken token);

    Task<GenericDbResponseViewModel<Guid?>> CreateQuizQuestionAsync(Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token);

    Task<GenericDbResponseViewModel<Guid?>> UpdateQuizQuestionAsync(Guid id, Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token);

    Task<List<QuizViewModel>> GetActiveQuizzesAsync(Guid userId, CancellationToken token);

    Task<GenericDbResponseViewModel<Guid?>> CreateQuizAttemptAsync(Guid quizId, Guid userId, Guid assignedByUserId, CancellationToken token);

    Task CompleteQuizAttemptAsync(Guid quizAttemptId, CompleteQuizAttemptRequestModel model, CancellationToken token);

    Task<List<QuizHistoryViewModel>> GetQuizzesAttemptHistoryAsync(Guid userId, CancellationToken token);

    Task<QuizAttemptHistoryViewModel?> GetQuizAttemptHistoryAsync(Guid id, Guid userId, CancellationToken token);
}