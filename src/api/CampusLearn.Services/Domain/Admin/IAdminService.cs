using CampusLearn.DataModel.Models.User;

namespace CampusLearn.Services.Domain.Admin;

public interface IAdminService
{
    Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetPendingRegistrationsAsync();

    Task<GenericAPIResponse<string>> ProcessRegistrationAsync(Guid userId, Guid accountStatusId, CancellationToken token);

    Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetUsersAsync();

    Task<GenericAPIResponse<string>> UpdateUserAsync(UserModel model);
}
