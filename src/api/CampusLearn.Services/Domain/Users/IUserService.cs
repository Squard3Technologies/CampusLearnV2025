using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.User;

namespace CampusLearn.Services.Domain.Users;

public interface IUserService
{
    Task<GenericAPIResponse<string>> LoginAsync(string emailAddress, string password);

    Task<GenericAPIResponse<CreateUserRequestModel>> CreateUserAccountAsync(CreateUserRequestModel user);

    Task<GenericAPIResponse<string>> ChangeUserPasswordAsync(Guid userId, string password);

    Task<UserProfileViewModel?> GetUserProfileAsync(Guid userId, CancellationToken token);

    Task UpdateUserProfileAsync(Guid userId, UserProfileRequestModel model, CancellationToken token);

    Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken token);

    Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetUsersAsync();
}
