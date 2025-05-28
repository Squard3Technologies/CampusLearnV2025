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
    public async Task<IActionResult> GetEnquiry(Guid id, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valid enquiryId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _enquiryService.GetEnquiry(userIdentifier.Value, id, token);
        return result != null ? Ok(result) : NoContent();
    }

    [HttpGet("active")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetActiveEnquiries(CancellationToken token)
    {
        var results = await _enquiryService.GetEnquiriesByStatus(EnquiryStatus.Pending, token);
        return results != null ? Ok(results) : NoContent();
    }

    [HttpPost("{id}/resolve")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> ResolveEnquiry(Guid id, [FromBody] ResolveEnquiryRequestModel model, CancellationToken token)
    {
        if (id == Guid.Empty)
            return BadRequest("Provide valid enquiryId");

        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        await _enquiryService.ResolveEnquiry(id, userIdentifier.Value, model, token);
        return Ok();
    }

    [HttpGet("resolved")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetResolvedEnquiries(CancellationToken token)
    {
        var results = await _enquiryService.GetEnquiriesByStatus(EnquiryStatus.Resolved, token);
        return results != null ? Ok(results) : NoContent();
    }
}
