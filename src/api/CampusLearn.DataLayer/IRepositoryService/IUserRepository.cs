namespace CampusLearn.DataLayer.IRepositoryService;

public interface IUserRepository
{
    Task<GenericDbResponseViewModel> LoginAsync(string emailAddress);

    Task<GenericDbResponseViewModel> CreateUserAccountAsync(CreateUserRequestModel user);

    Task<GenericDbResponseViewModel> ChangeUserPasswordAsync(Guid userId, string password);

    Task<UserProfileViewModel?> GetUserProfileAsync(Guid userId, CancellationToken token);

    Task UpdateUserProfileAsync(Guid userId, UserProfileRequestModel model, CancellationToken token);

    Task ChangePasswordAsync(Guid userId, string newPassword, CancellationToken token);

    Task<GenericDbResponseViewModel> GetUsersAsync();
}
