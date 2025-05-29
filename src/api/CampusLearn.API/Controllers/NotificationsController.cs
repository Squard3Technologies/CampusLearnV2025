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
    [MapToApiVersion(1)]
    public async Task<IActionResult> SendSMSMessageAsync(SendMessageRequest messageRequest)
    {
        var response = await _notificationService.SendMessageAsync(model: messageRequest, NotificationTypes.SMS, NotificationContentTypes.None);
        return Ok(response);
    }

    [HttpPost("email")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SendEmailMessageAsync(SendMessageRequest messageRequest)
    {
        var response = await _notificationService.SendMessageAsync(model: messageRequest, NotificationTypes.Email, NotificationContentTypes.None);
        return Ok(response);
    }
}