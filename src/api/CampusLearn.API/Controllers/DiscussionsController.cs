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
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetDiscussionsByTopic(Guid id)
    {
        return Ok();
    }

    [HttpGet("topic/{id}/comments")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetDiscussionComments(Guid id)
    {
        return Ok();
    }

    [HttpPost("topic/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateDiscussion(Guid id)
    {
        return Ok();
    }

    [HttpPost("topic/{id}/comment")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateComment(Guid id)
    {
        return Ok();
    }
}
