[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion(1)]
public class UserController : ControllerBase
{
    #region -- protected properties --

    protected readonly ILogger<UserController> logger;
    protected readonly IUserService _userService;

    #endregion -- protected properties --

    public UserController(ILogger<UserController> logger, IUserService userService)
    {
        this.logger = logger;
        _userService = userService;
    }

    [HttpPost("Login")]
    [MapToApiVersion(1)]
    public async Task<IActionResult> LoginAsync(LoginRequestModel loginRequest)
    {
        if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
        {
            return BadRequest("Both username and Password are required");
        }
        var response = await _userService.LoginAsync(loginRequest.Username, loginRequest.Password);
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

        response = await _userService.CreateUserAccountAsync(model);

        return Ok(response);
    }

    [HttpGet("profile")]
    [ProducesResponseType(typeof(UserProfileViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetUserProfile(CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        var result = await _userService.GetUserProfileAsync(userIdentifier.Value, token);

        return result != null ? Ok(result) : NoContent();
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UserProfileRequestModel model, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        await _userService.UpdateUserProfileAsync(userIdentifier.Value, model, token);
        return Ok();
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestModel model, CancellationToken token)
    {
        var userIdentifier = User.GetUserIdentifier();
        if (userIdentifier == null)
            return BadRequest("Could not determine UserIdentifier");

        await _userService.ChangePasswordAsync(userIdentifier.Value, model.NewPassword, token);
        return Ok();
    }
}
