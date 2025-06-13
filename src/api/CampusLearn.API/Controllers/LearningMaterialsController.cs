using CampusLearn.DataLayer.Constants;

using CampusLearn.DataModel.Models.LearningMaterial;

namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class LearningMaterialsController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;
    protected readonly IModuleService moduleService;
    protected readonly IConfiguration configuration;
    protected readonly MaterialStorageManager storageManager = new MaterialStorageManager();

    #endregion -- protected properties --

    public LearningMaterialsController(ILogger<UserController> logger, IModuleService moduleService, IConfiguration configuration)
    {
        this.logger = logger;
        this.moduleService = moduleService;
        this.configuration = configuration;
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("topic/{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetLearningMaterialsByTopic(Guid id)
    {
        return Ok();
    }

    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpPost("topic/uploads")]
    public async Task<IActionResult> UploadLearningMaterial(AddLearningMaterialRequest request)
    {
        var authUser = User.GetUserIdentifier();
        var baseUrl = $"{Request.Scheme}://{Request.Host}";

        if (request.FileData == null || request.FileData.Length == 0)
            return BadRequest("No file uploaded.");

        var filePath = await storageManager.UploadLearningMaterialAsync(configuration, baseUrl, request);

        if (string.IsNullOrEmpty(filePath))
            return BadRequest("Error saving file.");

        //save the details to the database.
        var materialViewModel = new LearningMaterialViewModel()
        {
            Id = Guid.NewGuid(),
            UserId = authUser.Value,
            TopicId = request.TopicId,
            FileType = request.FileType,
            FilePath = filePath,
            UploadedDate = DateTime.UtcNow
        };

        var apiResponse = await moduleService.AddLearningMaterialAsync(materialViewModel);

        return Ok(apiResponse);
    }


    [Authorize(Policy = AuthorizationRoles.Tutor)]
    [HttpDelete("{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> RemoveLearningMaterial(Guid id)
    {
        return Ok();
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet("{id}/download")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetLearningMaterial(Guid id)
    {
        return Ok();
    }
}
