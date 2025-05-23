using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Users;

public interface IUserService
{
    Task<GenericAPIResponse<string>> LoginAsync(string emailAddress, string password);

    Task<GenericAPIResponse<CreateUserRequestModel>> CreateUserAccountAsync(CreateUserRequestModel user);

    Task<GenericAPIResponse<string>> ChangeUserPasswordAsync(Guid userId, string password);
}
