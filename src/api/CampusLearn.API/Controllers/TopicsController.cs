using Asp.Versioning;
using CampusLearn.DataModel.Models.Topic;
using CampusLearn.Services.Domain.Modules;
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
    protected readonly IModuleService moduleService;

    #endregion

    public TopicsController(ILogger<TopicsController> logger, IModuleService moduleService)
    {
        this.logger = logger;
        this.moduleService = moduleService;
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [HttpPost("CreateTopic")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateTopicAsync([FromBody] CreateTopicRequest request)
    {
        var userId = User.GetUserIdentifier();
        var response = await moduleService.AddTopicAsync(userId.Value, request);
        return Ok(response);
    }


    [HttpGet("GetModuleTopics")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllTopicsAsync(Guid moduleId)
    {
        var responses = await moduleService.GetModuleTopicAsync(moduleId);
        return Ok(responses);
    }



}
