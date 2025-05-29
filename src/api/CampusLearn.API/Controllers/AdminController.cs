using CampusLearn.DataModel.Models.Modules;

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
        protected readonly IUserService userService;

        #endregion -- protected properties --

        public AdminController(ILogger<AdminController> logger, IAdminService adminService, IModuleService moduleService, IUserService userService)
        {
            this.logger = logger;
            this.adminService = adminService;
            this.moduleService = moduleService;
            this.userService = userService;
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
        public async Task<IActionResult> ChangeAccountStatusAsync([FromBody] ChangeAccountRequest request, CancellationToken token)
        {
            var apiResponse = await adminService.ProcessRegistrationAsync(userId: request.UserId, accountStatusId: request.AccountStatusId, token);
            return Ok(apiResponse);
        }

        #region -- user management --

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
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
        public async Task<IActionResult> ChangeUserAccountStatusAsync([FromBody] ChangeAccountRequest request, CancellationToken token)
        {
            var apiResponse = await adminService.ProcessRegistrationAsync(userId: request.UserId, accountStatusId: request.AccountStatusId, token);
            return Ok(apiResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("users/update")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserModel request)
        {
            var apiResponse = await adminService.UpdateUserAsync(request);
            return Ok(apiResponse);
        }

        #endregion -- user management --

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
        public async Task<IActionResult> UpdateModulesAsync([FromBody] CreateModuleRequest model)
        {
            var apiResponse = await moduleService.UpdateModuleAsync(model);
            return Ok(apiResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("modules/{id}/deactivate")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> DeactivateModuleAsync(Guid id)
        {
            var apiResponse = await moduleService.ChangeModuleStatusAsync(id, false);
            return Ok(apiResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost("modules/{id}/activate")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> ActivateModuleAsync(Guid id)
        {
            var apiResponse = await moduleService.ChangeModuleStatusAsync(id, true);
            return Ok(apiResponse);
        }

        #endregion -- MODULES API --
    }
}
