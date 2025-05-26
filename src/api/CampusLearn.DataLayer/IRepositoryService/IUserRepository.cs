namespace CampusLearn.DataLayer.IRepositoryService;

public interface IUserRepository
{
    Task<GenericDbResponseViewModel> LoginAsync(string emailAddress);

    Task<GenericDbResponseViewModel> CreateUserAccountAsync(CreateUserRequestModel user);

    Task<GenericDbResponseViewModel> ChangeUserPasswordAsync(Guid userId, string password);
}