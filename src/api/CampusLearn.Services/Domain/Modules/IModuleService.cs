using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Modules;

public interface IModuleService
{
    Task<GenericAPIResponse<string>> AddModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericAPIResponse<IEnumerable<ModuleViewModel>>> GetModulesAsync();

    Task<GenericAPIResponse<IEnumerable<UsersModuleViewModel>>> GetUserModulesAsync(Guid userId);
}
