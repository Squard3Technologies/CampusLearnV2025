using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IModuleRepository
{
    Task<GenericDbResponseViewModel> AddModuleAsync(ModuleViewModel module);

    Task<GenericDbResponseViewModel> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericDbResponseViewModel> GetModulesAsync();

    Task<GenericDbResponseViewModel> GetUserModulesAsync(Guid userId);
}
