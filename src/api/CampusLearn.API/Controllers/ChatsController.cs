namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class ChatsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> _logger;
    private readonly IChatService _chatService;

    #endregion -- protected properties --

    public ChatsController(ILogger<TopicsController> logger,
        IChatService chatService)
    {
        _logger = logger;
        _chatService = chatService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ChatViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetChats(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _chatService.GetChatsByUser(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("user/{id}")]
    [ProducesResponseType(typeof(List<ChatMessageViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetChat(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud UserId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _chatService.GetChatMessages(userIdentifier.Value, id, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("user/{id}/message")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateChatMessage(Guid id, [FromBody] CreateChatMessageRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud UserId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _chatService.CreateChatMessage(userIdentifier.Value, id, model, token);
        return result != null ? Ok(result) : NoContent();
    }
}
