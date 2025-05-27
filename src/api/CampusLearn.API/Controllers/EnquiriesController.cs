namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
[Authorize]
public class EnquiriesController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<TopicsController> _logger;
    private readonly IEnquiryService _enquiryService;

    #endregion -- protected properties --

    public EnquiriesController(ILogger<TopicsController> logger,
        IEnquiryService enquiryService)
    {
        _logger = logger;
        _enquiryService = enquiryService;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Tutor")]
    [HttpGet]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetEnquiries(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _enquiryService.GetEnquiries(userIdentifier.Value, token);
        return Ok(result);
    }

    [HttpPost]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateEnquiry(CreateEnquiryRequestModel model, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        await _enquiryService.CreateEnquiry(userIdentifier.Value, model, token);
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
