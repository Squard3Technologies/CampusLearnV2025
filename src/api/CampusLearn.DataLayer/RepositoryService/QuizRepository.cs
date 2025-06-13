using CampusLearn.DataModel.Models.Quizzes;
using Newtonsoft.Json;

namespace CampusLearn.DataLayer.RepositoryService;

public class QuizRepository : IQuizRepository
{
    #region -- protected properties --

    protected readonly ILogger<QuizRepository> _logger;
    protected readonly CampusLearnDbContext _database;

    #endregion -- protected properties --

    public QuizRepository(ILogger<QuizRepository> logger, CampusLearnDbContext database)
    {
        _logger = logger;
        _database = database;
    }

    public async Task<GenericDbResponseViewModel<Guid?>> CreateQuizAsync(Guid userId, CreateQuizRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("Name", model.Name, DbType.String);
            parameters.Add("Description", model.Description, DbType.String);
            parameters.Add("TopicId", model.TopicId, DbType.Guid);
            parameters.Add("Duration", model.Duration, DbType.Time);
            parameters.Add("CreatedByUserId", userId, DbType.Guid);
            parameters.Add("QuizId", null, DbType.Guid);

            var result = await db.QueryFirstAsync<GenericDbResponseViewModel<Guid?>>(
                "dbo.SP_UpsertQuiz",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<GenericDbResponseViewModel<Guid?>> CreateQuizAttemptAsync(Guid quizId, Guid userId, Guid assignedByUserId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("AssignedByUserId", assignedByUserId, DbType.Guid);
            parameters.Add("QuizId", quizId, DbType.Guid);

            var result = await db.QueryFirstAsync<GenericDbResponseViewModel<Guid?>>(
                "dbo.SP_CreateQuizAttempt",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task CompleteQuizAttemptAsync(Guid quizAttemptId, CompleteQuizAttemptRequestModel model, CancellationToken token)
    {
        var answersJson = JsonConvert.SerializeObject(model.QuestionAnswers);

        await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("AnswersJson", answersJson, DbType.String);
            parameters.Add("AttemptDuration", model.Duration, DbType.Time);
            parameters.Add("QuizAttemptId", quizAttemptId, DbType.Guid);

            var result = await db.ExecuteAsync(
                "dbo.SP_CompleteQuizAttempt",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );
        });
    }

    public async Task<GenericDbResponseViewModel<Guid?>> CreateQuizQuestionAsync(Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token)
    {
        var questionOptionsJson = JsonConvert.SerializeObject(model.QuestionOptions);

        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("QuizId", quizId, DbType.Guid);
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("Name", model.Name, DbType.String);
            parameters.Add("QuestionType", model.QuestionType, DbType.Int16);
            parameters.Add("QuestionOptionsJson", questionOptionsJson, DbType.String);

            var result = await db.QueryFirstAsync<GenericDbResponseViewModel<Guid?>>(
                "dbo.SP_UpsertQuizQuestion",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<List<QuizViewModel>> GetActiveQuizzesAsync(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var result = await db.QueryAsync<QuizViewModel>(
                "dbo.SP_GetActiveQuizzes",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result?.ToList() ?? [];
        });
    }

    public async Task<QuizAttemptHistoryViewModel?> GetQuizAttemptHistoryAsync(Guid id, Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("QuizAttemptId", id, DbType.Guid);

            var multipleResults = await db.QueryMultipleAsync(
                "dbo.SP_GetQuizAttemptHistory",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            var quizAttempt = await multipleResults.ReadFirstOrDefaultAsync<QuizAttemptHistoryViewModel>();
            if (quizAttempt == null)
                return null;

            var questions = (await multipleResults.ReadAsync<QuizAttemptHistoryQuestionViewModel>()).ToList();
            var attemptOptions = (await multipleResults.ReadAsync<QuizAttemptHistoryQuestionOptionViewModel>()).ToList();

            quizAttempt.Questions = questions;

            foreach (var question in quizAttempt.Questions)
            {
                question.QuestionOptions = attemptOptions
                    .Where(o => o.Id != Guid.Empty && o.QuestionId == question.Id)
                    .ToList();
            }

            return quizAttempt;
        });
    }

    public async Task<QuizDetailViewModel?> GetQuizDetailsAsync(Guid id, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("QuizId", id, DbType.Guid);

            var multipleResults = await db.QueryMultipleAsync(
                "dbo.SP_GetQuizDetail",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            var quiz = await multipleResults.ReadFirstOrDefaultAsync<QuizDetailViewModel>();
            if (quiz == null)
                return null;

            quiz.Questions = (await multipleResults.ReadAsync<QuizQuestionViewModel>()).ToList();
            var options = (await multipleResults.ReadAsync<QuizQuestionOptionViewModel>()).ToList();

            foreach (var question in quiz.Questions)
            {
                question.QuestionOptions = options
                    .Where(o => o.Id != Guid.Empty && o.QuestionId == question.Id)
                    .ToList();
            }

            return quiz;
        });
    }

    public async Task<List<QuizHistoryViewModel>> GetQuizzesAttemptHistoryAsync(Guid userId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Guid);

            var result = await db.QueryAsync<QuizHistoryViewModel>(
                "dbo.SP_GetQuizzesAttemptHistory",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result?.ToList() ?? [];
        });
    }

    public async Task<List<QuizViewModel>> GetQuizzesByTopicAsync(Guid topicId, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("TopicId", topicId, DbType.Guid);

            var results = await db.QueryAsync<QuizViewModel>(
                "dbo.SP_GetQuizzesByTopic",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return results?.ToList() ?? [];
        });
    }

    public async Task<GenericDbResponseViewModel<Guid?>> UpdateQuizAsync(Guid id, Guid userId, CreateQuizRequestModel model, CancellationToken token)
    {
        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("QuizId", id, DbType.Guid);
            parameters.Add("Name", model.Name, DbType.String);
            parameters.Add("Description", model.Description, DbType.String);
            parameters.Add("TopicId", model.TopicId, DbType.Guid);
            parameters.Add("Duration", model.Duration, DbType.Time);
            parameters.Add("CreatedByUserId", userId, DbType.Guid);

            var result = await db.QueryFirstAsync<GenericDbResponseViewModel<Guid?>>(
                "dbo.SP_UpsertQuiz",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }

    public async Task<GenericDbResponseViewModel<Guid?>> UpdateQuizQuestionAsync(Guid id, Guid quizId, Guid userId, CreateQuizQuestionRequestModel model, CancellationToken token)
    {
        var questionOptionsJson = JsonConvert.SerializeObject(model.QuestionOptions);

        return await _database.ExecuteTransactionAsync(async (db, transaction) =>
        {
            var parameters = new DynamicParameters();
            parameters.Add("QuestionId", id, DbType.Guid);
            parameters.Add("QuizId", quizId, DbType.Guid);
            parameters.Add("UserId", userId, DbType.Guid);
            parameters.Add("Name", model.Name, DbType.String);
            parameters.Add("QuestionType", model.QuestionType, DbType.Int16);
            parameters.Add("QuestionOptionsJson", questionOptionsJson, DbType.String);

            var result = await db.QueryFirstAsync<GenericDbResponseViewModel<Guid?>>(
                "dbo.SP_UpsertQuizQuestion",
                parameters,
                transaction,
                commandTimeout: 360,
                commandType: CommandType.StoredProcedure
            );

            return result;
        });
    }
}