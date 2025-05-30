namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class DiscussionsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;

    #endregion -- protected properties --

    public DiscussionsController(ILogger<UserController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("topic/{id}")]
    [ProducesResponseType(typeof(List<DiscussionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDiscussionsByTopic(Guid id)
    {
        return Ok();
    }

    [HttpGet("{id}/comments")]
    [ProducesResponseType(typeof(List<DiscussionCommentViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetDiscussionComments(Guid id)
    {
        return Ok();
    }

    [HttpPost("topic/{id}")]
    [ProducesResponseType(typeof(Guid?), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateDiscussion(Guid id, [FromBody] CreateDiscussionRequestModel model)
    {
        return Ok();
    }

    [HttpPost("{id}/comment")]
    [ProducesResponseType(typeof(Guid?), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateComment(Guid id, [FromBody] CreateDiscussionCommentRequestModel model)
    {
        return Ok();
    }
}
