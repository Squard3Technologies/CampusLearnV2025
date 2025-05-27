namespace CampusLearn.API.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class UserController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;
    protected readonly IUserService userService;

    #endregion -- protected properties --

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        this.logger = logger;
        this.userService = userService;
    }

    [HttpPost("Login")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> LoginAsync(LoginRequestModel loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
        {
            return BadRequest("Both username and Password are required");
        }
        var response = await userService.LoginAsync(loginRequest.Username, loginRequest.Password);
        return Ok(response);
    }

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Learner, Tutor")]
    [HttpPost("CreateAccount")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateUserRequestModel model)
    {
        var response = new GenericAPIResponse<CreateUserRequestModel>();
        if (model == null)
        {
            response.Status = false;
            response.StatusCode = 404;
            response.StatusMessage = "User details missing";
            return BadRequest(response);
        }
        if (string.IsNullOrEmpty(model.EmailAddress))
        {
            response.Status = false;
            response.StatusCode = 404;
            response.StatusMessage = "User email address is required";
            return BadRequest(response);
        }

        response = await userService.CreateUserAccountAsync(model);

        return Ok(response);
    }

    [HttpGet("profile")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> GetUserProfile()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }

    [HttpPut("profile")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> UpdateUserProfile()
    {
        var userIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return Ok();
    }
}
