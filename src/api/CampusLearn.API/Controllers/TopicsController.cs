using CampusLearn.DataLayer.Constants;
using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class TopicsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> logger;
    protected readonly IModuleService moduleService;

    #endregion -- protected properties --

    public TopicsController(ILogger<TopicsController> logger, IModuleService moduleService)
    {
        this.logger = logger;
        this.moduleService = moduleService;
    }

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPost]
    //[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateTopicAsync([FromBody] CreateTopicRequest request, CancellationToken token)
    {
        var userId = User.GetUserIdentifier();
        var response = await moduleService.AddTopicAsync(userId.Value, request, token);
        return response?.Body != null ? Ok(response) : NoContent();
    }

    [HttpGet("modules/{moduleId}")]
    [ProducesResponseType(typeof(GenericAPIResponse<IEnumerable<TopicViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTopicsAsync(Guid moduleId)
    {
        var responses = await moduleService.GetModuleTopicAsync(moduleId);
        return Ok(responses);
    }
}
