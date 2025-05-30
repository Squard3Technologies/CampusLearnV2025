namespace CampusLearn.DataLayer.IRepositoryService;

public interface IAdminRepository
{
    Task<GenericDbResponseViewModel> GetPendingRegistrationsAsync();

    Task<GenericDbResponseViewModel> ChangeUserAccountStatusAsync(Guid userId, Guid accountStatusId);

    Task<GenericDbResponseViewModel> GetUsersAsync();
    Task<GenericDbResponseViewModel> UpdateUserAsync(UserViewModel model);
}
