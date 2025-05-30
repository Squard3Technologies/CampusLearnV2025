namespace CampusLearn.Services.Domain.Authorization;

public class HierarchicalRoleRequirement : IAuthorizationRequirement
{
    public UserRoles RequiredRole { get; }

    public HierarchicalRoleRequirement(UserRoles requiredRole)
    {
        RequiredRole = requiredRole;
    }
}
