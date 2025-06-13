namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class DiscussionsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;
    private readonly IDiscussionService _discussionService;

    #endregion -- protected properties --

    public DiscussionsController(ILogger<UserController> logger,
        IDiscussionService discussionService)
    {
        this.logger = logger;
        _discussionService = discussionService;
    }

    [HttpGet("topic/{id}")]
    [ProducesResponseType(typeof(List<DiscussionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDiscussionsByTopic(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        var results = await _discussionService.GetDiscussionsByTopic(id, token);

        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(List<DiscussionCommentViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDiscussionComments(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud DiscussionId");

        var results = await _discussionService.GetDiscussionComments(id, token);

        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("topic/{id}")]
    [ProducesResponseType(typeof(Guid?), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDiscussion(Guid id, [FromBody] CreateDiscussionRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _discussionService.CreateDiscussion(id, userIdentifier.Value, model, token);

        return result != null ? Ok(result) : NoContent();
    }

    [HttpPost("{id}/comment")]
    [ProducesResponseType(typeof(Guid?), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateComment(Guid id, [FromBody] CreateDiscussionCommentRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _discussionService.CreateComment(id, userIdentifier.Value, model, token);

        return result != null ? Ok(result) : NoContent();
    }
}
