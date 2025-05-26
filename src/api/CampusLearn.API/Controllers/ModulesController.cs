using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace CampusLearn.API.Controllers;


[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class ModulesController : ControllerBase
{

    #region -- protected properties --
    protected readonly ILogger<ModulesController> logger;
    //protected readonly IMessagingServices messagingServices;

    #endregion

    public ModulesController(ILogger<ModulesController> logger)
    {
        this.logger = logger;        
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Tutor")]
    [HttpPost("CreateModule")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateModuleAsync()
    {
        return Ok();
    }


    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Tutor")]
    [HttpPost("LearnerModuleLinkage")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> LinkLearnersToModuleAsync()
    {
        return Ok();
    }



    [HttpGet("GetAllModules")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetAllModulesAsync()
    {
        return Ok();
    }

}
