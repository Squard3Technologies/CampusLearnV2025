namespace CampusLearn.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class NotificationsController : ControllerBase
{
    private readonly ILogger<NotificationsController> _logger;
    private readonly INotificationService _notificationService;

    public NotificationsController(ILogger<NotificationsController> logger, INotificationService notificationService)
    {
        _logger = logger;
        _notificationService = notificationService;
    }

    [HttpPost("sms")]
    [ProducesResponseType(typeof(GenericDbResponseViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendSMSMessageAsync(SendMessageRequest messageRequest, CancellationToken token)
    {
        var response = await _notificationService.SendMessageAsync(model: messageRequest, NotificationTypes.SMS, NotificationContentTypes.None, token);
        return Ok(response);
    }

    [HttpPost("email")]
    [ProducesResponseType(typeof(GenericDbResponseViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> SendEmailMessageAsync(SendMessageRequest messageRequest, CancellationToken token)
    {
        var response = await _notificationService.SendMessageAsync(model: messageRequest, NotificationTypes.Email, NotificationContentTypes.None, token);
        return Ok(response);
    }
}
