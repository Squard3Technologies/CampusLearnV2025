using CampusLearn.DataModel.Models.User;
using CampusLearn.DataModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IUserRepository
{
    Task<GenericDbResponseViewModel> LoginAsync(string emailAddress);

    Task<GenericDbResponseViewModel> CreateUserAccountAsync(CreateUserRequestModel user);

    Task<GenericDbResponseViewModel> ChangeUserPasswordAsync(Guid userId, string password);
}
