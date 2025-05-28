using CampusLearn.DataModel.Models.Quizzes;
using CampusLearn.Services.Domain.Quizzes;

namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class QuizzesController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> _logger;
    private readonly IQuizService _quizService;

    #endregion -- protected properties --

    public QuizzesController(ILogger<TopicsController> logger,
        IQuizService quizService)
    {
        _logger = logger;
        _quizService = quizService;
    }

    [HttpGet("topic/{topicId}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizzesByTopic([FromQuery] Guid topicId, CancellationToken token)
    {
        if (topicId == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        var results = await _quizService.GetQuizzesByTopic(topicId, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("{id}/details")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizDetails(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        var result = await _quizService.GetQuizDetails(id, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpPost]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequestModel model, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _quizService.CreateQuiz(userIdentifier.Value, model, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpPut("{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateQuiz(Guid id, [FromBody] CreateQuizRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        await _quizService.UpdateQuiz(id, userIdentifier.Value, model, token);
        return Ok();
    }

    [HttpPost("{id}/questions")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateQuizQuestion(Guid id, [FromBody] CreateQuizQuestionRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _quizService.CreateQuizQuestion(id, userIdentifier.Value, model, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpPut("{id}/questions/{questionId}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateQuizQuestion(Guid id, Guid questionId, [FromBody] CreateQuizQuestionRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        if (questionId == Guid.Empty)
            return BadRequest("Provide valud QuestionId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        await _quizService.UpdateQuizQuestion(questionId, id, userIdentifier.Value, model, token);
        return Ok();
    }

    [HttpGet("active")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetActiveQuizzes(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _quizService.GetActiveQuizzes(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("{id}/attempt")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateQuizAttempt(Guid id, [FromBody] CreateQuizAttemptRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _quizService.CreateQuizAttempt(id, userIdentifier.Value, model, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpGet("attempt-history")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizzesAttemptHistory(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _quizService.GetQuizzesAttemptHistory(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("attempt-history/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizAttemptHistory(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizAttemptId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _quizService.GetQuizAttemptHistory(id, userIdentifier.Value, token);
        return result != null ? Ok(result) : NoContent();
    }
}
