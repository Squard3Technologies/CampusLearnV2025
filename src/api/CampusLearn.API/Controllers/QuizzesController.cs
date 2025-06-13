using CampusLearn.DataLayer.Constants;
using CampusLearn.DataModel.Models.Quizzes;

namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class QuizzesController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<QuizzesController> _logger;
    private readonly IQuizService _quizService;

    #endregion -- protected properties --

    public QuizzesController(ILogger<QuizzesController> logger,
        IQuizService quizService)
    {
        _logger = logger;
        _quizService = quizService;
    }

    [HttpGet("topic/{topicId}")]
    [ProducesResponseType(typeof(List<QuizViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetQuizzesByTopic(Guid topicId, CancellationToken token)
    {
        if (topicId == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        var results = await _quizService.GetQuizzesByTopic(topicId, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("{id}/details")]
    [ProducesResponseType(typeof(QuizDetailViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetQuizDetails(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        var result = await _quizService.GetQuizDetails(id, token);
        return result != null ? Ok(result) : NoContent();
    }

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateQuiz([FromBody] CreateQuizRequestModel model, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _quizService.CreateQuiz(userIdentifier.Value, model, token);
        return result != null ? Ok(result) : NoContent();
    }

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPost("{id}/questions")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPut("{id}/questions/{questionId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(typeof(List<QuizViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetActiveQuizzes(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _quizService.GetActiveQuizzes(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("{id}/attempt")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateQuizAttempt(Guid id, [FromQuery] Guid? assignedByUserId, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _quizService.CreateQuizAttempt(id, userIdentifier.Value, assignedByUserId ?? userIdentifier.Value, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpPut("attempt/{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteQuizAttempt(Guid id, [FromBody] CompleteQuizAttemptRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud QuizId");

        await _quizService.CreateQuizAttempt(id, model, token);
        return Ok();
    }

    [HttpGet("attempt-history")]
    [ProducesResponseType(typeof(List<QuizHistoryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetQuizzesAttemptHistory(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _quizService.GetQuizzesAttemptHistory(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("attempt-history/{id}")]
    [ProducesResponseType(typeof(QuizAttemptHistoryViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
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