using CampusLearn.Services.Domain.Autherization;

namespace CampusLearn.Services.Domain.Authorization;

public class HierarchicalRoleHandler : AuthorizationHandler<HierarchicalRoleRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HierarchicalRoleRequirement requirement)
    {
        var userRole = context.User.FindFirst(ClaimTypes.Role)?.Value;
        if (Enum.TryParse<UserRoles>(userRole, out var parsedRole) && RoleHierarchy.HasAccess(parsedRole, requirement.RequiredRole))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
