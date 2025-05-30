using CampusLearn.DataModel.Models.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IModuleRepository
{
    Task<GenericDbResponseViewModel> AddModuleAsync(ModuleViewModel module);
    Task<GenericDbResponseViewModel> UpdateModuleAsync(ModuleViewModel module);

    Task<GenericDbResponseViewModel> ChangeModuleStatusAsync(Guid moduleId, bool status);

    Task<GenericDbResponseViewModel> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericDbResponseViewModel> GetModulesAsync();

    Task<GenericDbResponseViewModel> GetUserModulesAsync(Guid userId);


    #region -- topic section --

    Task<GenericDbResponseViewModel> AddTopicAsync(Guid userId, CreateTopicRequest module);
    Task<GenericDbResponseViewModel> GetModuleTopicAsync(Guid moduleId);

    #endregion
}
