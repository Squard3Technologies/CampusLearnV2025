namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class EnquiriesController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> _logger;

    #endregion -- protected properties --

    public EnquiriesController(ILogger<TopicsController> logger)
    {
        _logger = logger;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Tutor")]
    [HttpGet]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetEnquiries()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }

    [HttpPost]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateEnquiry()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetEnquiry(Guid id)
    {
        return Ok();
    }

    [HttpGet("active")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetActiveEnquiries()
    {
        return Ok();
    }

    [HttpGet("{id}/resolve")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ResolveEnquiry(Guid id)
    {
        return Ok();
    }

    [HttpGet("resolved")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetResolvedEnquiries()
    {
        return Ok();
    }
}
