namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class QuizzesController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> _logger;

    #endregion -- protected properties --

    public QuizzesController(ILogger<TopicsController> logger)
    {
        _logger = logger;
    }

    [HttpGet("topic/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizzesByTopic([FromQuery] Guid id)
    {
        return Ok();
    }

    [HttpGet("{id}/details")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetChat(Guid id)
    {
        return Ok();
    }

    [HttpPost]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateQuiz()
    {
        return Ok();
    }

    [HttpPut("{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateQuiz(Guid id)
    {
        return Ok();
    }

    [HttpGet("active")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetActiveQuizzes()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }

    [HttpPost("attempt")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateQuizAttempt()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }

    [HttpGet("attempt-history")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizzesAttemptHistory()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }

    [HttpGet("attempt-history/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetQuizAttemptHistory()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }
}
