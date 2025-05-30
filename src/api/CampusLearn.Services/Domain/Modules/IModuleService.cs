using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.Modules;
using CampusLearn.DataModel.Models.Topic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Modules;

public interface IModuleService
{
    Task<GenericAPIResponse<string>> AddModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> UpdateModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> ChangeModuleStatusAsync(Guid moduleId, bool status);

    Task<GenericAPIResponse<string>> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericAPIResponse<IEnumerable<ModuleViewModel>>> GetModulesAsync();

    Task<GenericAPIResponse<IEnumerable<UsersModuleViewModel>>> GetUserModulesAsync(Guid userId);


    #region -- topic section --

    Task<GenericAPIResponse<string>> AddTopicAsync(Guid userId, CreateTopicRequest module);
    Task<GenericAPIResponse<IEnumerable<TopicViewModel>>> GetModuleTopicAsync(Guid moduleId);

    #endregion

}
