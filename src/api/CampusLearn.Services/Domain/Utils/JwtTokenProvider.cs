namespace CampusLearn.Services.Domain.Utils;

public class JwtTokenProvider(IConfiguration configuration)
{
    public string CreateToken(UserViewModel user)
    {
        string secretKey = configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, algorithm: SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.EmailAddress),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    //public UserViewModel GetUserViewModel()
    //{
    //    var identity = HttpContext.User.Identity as ClaimsIdentity;

    //    // Gets list of claims.
    //    IEnumerable<Claim> claim = identity.Claims;
    //}

    private List<UserRoles> GetHierarchicalRoles(UserRoles role)
    {
        return role switch
        {
            UserRoles.Administrator => [UserRoles.Administrator, UserRoles.Lecturer, UserRoles.Tutor, UserRoles.Learner],
            UserRoles.Lecturer => [UserRoles.Lecturer, UserRoles.Tutor, UserRoles.Learner],
            UserRoles.Tutor => [UserRoles.Tutor, UserRoles.Learner],
            UserRoles.Learner => [UserRoles.Learner],
            _ => []
        };
    }
}
