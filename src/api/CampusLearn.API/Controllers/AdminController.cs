using CampusLearn.DataModel.Models.Modules;
using CampusLearn.Services.Domain.Admin;
using CampusLearn.Services.Domain.Modules;
using CampusLearn.Services.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CampusLearn.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1)]
    public class AdminController : ControllerBase
    {
        #region -- protected properties --

        protected readonly ILogger<AdminController> logger;
        protected readonly IAdminService adminService;
        protected readonly IModuleService moduleService;

        #endregion -- protected properties --

        public AdminController(ILogger<AdminController> logger, IAdminService adminService, IModuleService moduleService)
        {
            this.logger = logger;
            this.adminService = adminService;
            this.moduleService = moduleService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpGet("registrations")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetPendingRegistrationsAsync()
        {
            var apiResponse = await adminService.GetPendingRegistrationsAsync();            
            return Ok(apiResponse);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("registrations/status")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> ChangeAccountStatusAsync([FromBody] ChangeAccountRequest request)
        {
            var apiResponse = await adminService.ProcessRegistrationAsync(userId: request.UserId, accountStatusId: request.AccountStatusId);            
            return Ok(apiResponse);
        }


        #region -- user management --

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("users")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var apiResponse = await adminService.GetUsersAsync();
            return Ok(apiResponse);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("users/status")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> ChangeUserAccountStatusAsync([FromBody] ChangeAccountRequest request)
        {
            var apiResponse = await adminService.ProcessRegistrationAsync(userId: request.UserId, accountStatusId: request.AccountStatusId);
            return Ok(apiResponse);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("users/manage")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> ManageUserAsync([FromBody] UserModel request)
        {

            return Ok();
        }

        #endregion



        #region -- MODULES API --

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("modules")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> AddModuleAsync([FromBody] CreateModuleRequest request)
        {
            var apiResponse = await moduleService.AddModuleAsync(request);
            return Ok(apiResponse);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpGet("modules")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> GetModulesAsync()
        {
            var apiResponse = await moduleService.GetModulesAsync();
            return Ok(apiResponse);
        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPut("modules")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> UpdateModulesAsync([FromBody] ModuleViewModel model)
        {
            var apiResponse = await moduleService.GetModulesAsync();
            return Ok(apiResponse);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("modules/{id}/deactivate")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> DeactivateModuleAsync(Guid id)
        {
            var apiResponse = await moduleService.GetModulesAsync();
            return Ok(apiResponse);
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("modules/{id}/activate")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> ActivateModuleAsync(Guid id)
        {
            var apiResponse = await moduleService.GetModulesAsync();
            return Ok(apiResponse);
        }


        #endregion

    }
}
