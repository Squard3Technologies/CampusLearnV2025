using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.User;
using CampusLearn.DataModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Admin;

public interface IAdminService
{
    Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetPendingRegistrationsAsync();

    Task<GenericAPIResponse<string>> ProcessRegistrationAsync(Guid userId, Guid accountStatusId);

    Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetUsersAsync();

}
