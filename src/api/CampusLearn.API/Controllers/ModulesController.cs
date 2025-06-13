using CampusLearn.DataLayer.Constants;
using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class ModulesController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<ModulesController> logger;
    protected readonly IModuleService moduleService;

    #endregion -- protected properties --

    public ModulesController(ILogger<ModulesController> logger, IModuleService moduleService)
    {
        this.logger = logger;
        this.moduleService = moduleService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ModuleViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetModules(CancellationToken token)
    {
        var userId = User.GetUserIdentifier();
        var response = await moduleService.GetUserModulesAsync(userId.Value);
        return response?.Body != null ? Ok(response.Body) : NoContent();
    }

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPost("{moduleId}/topic")]
    //[ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CreateTopicAsync(Guid moduleId, [FromBody] CreateTopicRequest request, CancellationToken token)
    {
        var userId = User.GetUserIdentifier();
        var response = await moduleService.AddTopicAsync(userId.Value, moduleId, request, token);
        return response?.Body != null ? Ok(response) : NoContent();
    }

    [HttpGet("{moduleId}/topics")]
    [ProducesResponseType(typeof(GenericAPIResponse<IEnumerable<TopicViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTopicsAsync(Guid moduleId)
    {
        var responses = await moduleService.GetModuleTopicsAsync(moduleId);
        return Ok(responses);
    }

    [HttpGet("{moduleId}/topics/{topicId}")]
    [ProducesResponseType(typeof(GenericAPIResponse<TopicViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllTopicsAsync(Guid moduleId, Guid topicId)
    {
        var responses = await moduleService.GetModuleTopicAsync(moduleId, topicId);
        return Ok(responses);
    }
}