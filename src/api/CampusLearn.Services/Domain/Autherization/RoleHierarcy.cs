namespace CampusLearn.Services.Domain.Autherization;

public static class RoleHierarchy
{
    private static readonly Dictionary<UserRoles, List<UserRoles>> Hierarchy = new()
    {
        { UserRoles.Administrator, new() { UserRoles.Administrator, UserRoles.Lecturer, UserRoles.Tutor, UserRoles.Learner } },
        { UserRoles.Lecturer,     new() { UserRoles.Lecturer, UserRoles.Tutor, UserRoles.Learner } },
        { UserRoles.Tutor,        new() { UserRoles.Tutor, UserRoles.Learner } },
        { UserRoles.Learner,      new() { UserRoles.Learner } }
    };

    public static bool HasAccess(UserRoles userRole, UserRoles requiredRole)
    {
        return Hierarchy.TryGetValue(userRole, out var roles) && roles.Contains(requiredRole);
    }
}
