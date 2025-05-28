using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IAdminRepository
{
    Task<GenericDbResponseViewModel> GetPendingRegistrationsAsync();

    Task<GenericDbResponseViewModel> ChangeUserAccountStatusAsync(Guid userId, Guid accountStatusId);

    Task<GenericDbResponseViewModel> GetUsersAsync();
}
