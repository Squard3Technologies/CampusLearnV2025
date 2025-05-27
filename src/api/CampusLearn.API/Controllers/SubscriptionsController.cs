namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class SubscriptionsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;

    #endregion -- protected properties --

    public SubscriptionsController(ILogger<UserController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("/tutors")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetSubscribedTutors()
    {
        return Ok();
    }

    [HttpPost("/tutors")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateSubscribedTutor()
    {
        return Ok();
    }

    [HttpDelete("/tutors/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveSubscriberTutor(Guid id)
    {
        return Ok();
    }

    [HttpGet("/topics")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetSubscribedTopics()
    {
        return Ok();
    }

    [HttpPost("/topics")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateSubscribedTopics()
    {
        return Ok();
    }

    [HttpDelete("/topics/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveSubscriberTopics(Guid id)
    {
        return Ok();
    }
}
