namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class ChatsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> _logger;

    #endregion -- protected properties --

    public ChatsController(ILogger<TopicsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetChats()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }

    [HttpGet("user/{userId}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetChat(Guid userId)
    {
        return Ok();
    }

    [HttpGet("users")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetChatUsers([FromQuery] string search)
    {
        return Ok();
    }
}
