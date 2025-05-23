using CampusLearn.DataModel.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace CampusLearn.Services.Domain.Utils;

public class JwtTokenProvider(IConfiguration configuration)
{
    public string CreateToken(UserViewModel user)
    {
        string secretKey = configuration["Jwt:Secret"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, algorithm: SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Subject = new ClaimsIdentity(
                [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName.ToString()),
                new Claim(ClaimTypes.Surname, user.Surname.ToString()),
                //new Claim(JwtRegisteredClaimNames.FamilyName, user.Surname.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            ]),
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
}
