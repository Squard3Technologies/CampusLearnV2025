using CampusLearn.Services.Domain.Subscriptions;

namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class SubscriptionsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;
    private readonly ISubscriptionService _subscriptionService;

    #endregion -- protected properties --

    public SubscriptionsController(ILogger<UserController> logger,
        ISubscriptionService subscriptionService)
    {
        this.logger = logger;
        _subscriptionService = subscriptionService;
    }

    [HttpGet("tutors")]
    [ProducesResponseType(typeof(List<TutorSubscriptionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSubscribedTutors(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _subscriptionService.GetTutors(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("tutors{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateSubscribedTutor(Guid id, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        if (id == Guid.Empty)
            return BadRequest("Provide valud TutorId");

        var result = await _subscriptionService.SubscribeToTutor(userIdentifier.Value, id, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpDelete("tutors/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveSubscriberTutor(Guid id, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        if (id == Guid.Empty)
            return BadRequest("Provide valud TutorId");

        await _subscriptionService.UnsubscribeFromTutor(userIdentifier.Value, id, token);
        return Ok();
    }

    [HttpGet("tutors/available")]
    [ProducesResponseType(typeof(List<TutorSubscriptionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAvailableTutors(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _subscriptionService.GetAvailableTutors(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpGet("topics")]
    [ProducesResponseType(typeof(List<TopicSubscriptionViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetSubscribedTopics(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var results = await _subscriptionService.GetTopics(userIdentifier.Value, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("topics{id}")]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateSubscribedTopics(Guid id, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        if (id == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        var result = await _subscriptionService.SubscribeToTopic(userIdentifier.Value, id, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpDelete("topics/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> RemoveSubscriberTopics(Guid id, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        if (id == Guid.Empty)
            return BadRequest("Provide valud TopicId");

        await _subscriptionService.UnsubscribeFromTopic(userIdentifier.Value, id, token);
        return Ok();
    }
}
