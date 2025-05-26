using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace CampusLearn.API.Controllers;



[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class TopicsController : ControllerBase
{
    #region -- protected properties --
    protected readonly ILogger<TopicsController> logger;
    //protected readonly IMessagingServices messagingServices;

    #endregion

    public TopicsController(ILogger<TopicsController> logger)
    {
        this.logger = logger;
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Tutor")]
    [HttpPost("CreateTopic")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateTopicAsync()
    {
        return Ok();
    }


    [HttpGet("GetAllTopics")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllTopicsAsync()
    {
        return Ok();
    }



}
