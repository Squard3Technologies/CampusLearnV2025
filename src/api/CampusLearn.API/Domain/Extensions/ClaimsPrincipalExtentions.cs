namespace CampusLearn.API.Domain.Extensions;

public static class ClaimsPrincipalExtentions
{
    public static Guid? GetUserIdentifier(this ClaimsPrincipal user)
    {
        return Guid.TryParse(user?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out Guid result) ? result : null;
    }
}
