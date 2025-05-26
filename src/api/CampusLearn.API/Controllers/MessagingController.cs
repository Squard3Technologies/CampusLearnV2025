namespace CampusLearn.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class MessagingController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<MessagingController> logger;
    protected readonly IMessagingServices messagingServices;

    #endregion -- protected properties --

    public MessagingController(ILogger<MessagingController> logger, IMessagingServices messagingServices)
    {
        this.logger = logger;
        this.messagingServices = messagingServices;
    }

    [HttpPost("SendSMS")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SendSMSMessageAsync(SendMessageRequest messageRequest)
    {
        var response = await messagingServices.SendMessageAsync(model: messageRequest, messageType: "SMS");
        return Ok(response);
    }

    [HttpPost("SendEMAIL")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> SendEmailMessageAsync(SendMessageRequest messageRequest)
    {
        var response = await messagingServices.SendMessageAsync(model: messageRequest, messageType: "EMAIL");
        return Ok(response);
    }
}