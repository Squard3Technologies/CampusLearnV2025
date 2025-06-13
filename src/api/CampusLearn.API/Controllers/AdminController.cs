using CampusLearn.DataLayer.Constants;
using CampusLearn.DataModel.Models.Modules;

namespace CampusLearn.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion(1)]
    [Authorize]
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

        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpGet("registrations")]
        [ProducesResponseType(typeof(GenericAPIResponse<IEnumerable<UserViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPendingRegistrationsAsync()
        {
            var apiResponse = await adminService.GetPendingRegistrationsAsync();
            return Ok(apiResponse);
        }

        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPost("registrations/status")]
        [ProducesResponseType(typeof(GenericAPIResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeAccountStatusAsync([FromBody] ChangeAccountRequest request, CancellationToken token)
        {
            var apiResponse = await adminService.ProcessRegistrationAsync(userId: request.UserId, accountStatusId: request.AccountStatusId, token);
            return Ok(apiResponse);
        }

        #region -- user management --

        [HttpGet("users")]
        [ProducesResponseType(typeof(GenericAPIResponse<IEnumerable<UserViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var apiResponse = await adminService.GetUsersAsync();
            return Ok(apiResponse);
        }



        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPost("users/status")]
        [ProducesResponseType(typeof(GenericAPIResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangeUserAccountStatusAsync([FromBody] ChangeAccountRequest request, CancellationToken token)
        {
            var apiResponse = await adminService.ProcessRegistrationAsync(userId: request.UserId, accountStatusId: request.AccountStatusId, token);
            return Ok(apiResponse);
        }



        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPost("users/update")]
        [ProducesResponseType(typeof(GenericAPIResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserModel request)
        {
            var apiResponse = await adminService.UpdateUserAsync(request);
            return Ok(apiResponse);
        }

        #endregion



        #region -- MODULES API --

        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPost("modules")]
        [ProducesResponseType(typeof(GenericAPIResponse<Guid?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddModuleAsync([FromBody] CreateModuleRequest request)
        {
            var apiResponse = await moduleService.AddModuleAsync(request);
            return Ok(apiResponse);
        }

        [HttpGet("modules")]
        [ProducesResponseType(typeof(GenericAPIResponse<IEnumerable<ModuleViewModel>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetModulesAsync()
        {
            var apiResponse = await moduleService.GetModulesAsync();
            return Ok(apiResponse);
        }

        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPut("modules")]
        [ProducesResponseType(typeof(GenericAPIResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateModulesAsync([FromBody] CreateModuleRequest model)
        {
            var apiResponse = await moduleService.UpdateModuleAsync(model);
            return Ok(apiResponse);
        }

        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPost("modules/{id}/deactivate")]
        [ProducesResponseType(typeof(GenericAPIResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeactivateModuleAsync(Guid id)
        {
            var apiResponse = await moduleService.ChangeModuleStatusAsync(id, false);
            return Ok(apiResponse);
        }

        [Authorize(Policy = AuthorizationRoles.Administrator)]
        [HttpPost("modules/{id}/activate")]
        [ProducesResponseType(typeof(GenericAPIResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ActivateModuleAsync(Guid id)
        {
            var apiResponse = await moduleService.ChangeModuleStatusAsync(id, true);
            return Ok(apiResponse);
        }

        #endregion -- MODULES API --
    }
}
