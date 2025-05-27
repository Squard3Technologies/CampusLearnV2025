namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class LearningMaterialsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;

    #endregion -- protected properties --

    public LearningMaterialsController(ILogger<UserController> logger)
    {
        this.logger = logger;
    }

    [HttpGet("topic/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetLearningMaterialsByTopic(Guid id)
    {
        return Ok();
    }

    [HttpPost("topic/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateLearningMaterial(Guid id)
    {
        return Ok();
    }

    [HttpDelete("{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveLearningMaterial(Guid id)
    {
        return Ok();
    }

    [HttpGet("{id}/download")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetLearningMaterial(Guid id)
    {
        return Ok();
    }
}
