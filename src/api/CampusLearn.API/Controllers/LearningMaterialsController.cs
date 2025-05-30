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
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateLearningMaterial(Guid id, IFormFile file)
    {
        var authUser = User.GetUserIdentifier();

        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "API", "Uploads");

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var filePath = Path.Combine(uploadPath, file.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        //save the details to the database.

        return Ok(new { filePath });
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
