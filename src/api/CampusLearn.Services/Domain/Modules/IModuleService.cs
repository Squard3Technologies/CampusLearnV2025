using CampusLearn.DataModel.Models.Modules;
using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.Services.Domain.Modules;

public interface IModuleService
{
    Task<GenericAPIResponse<Guid?>> AddModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> UpdateModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> ChangeModuleStatusAsync(Guid moduleId, bool status);

    Task<GenericAPIResponse<string>> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericAPIResponse<IEnumerable<ModuleViewModel>>> GetModulesAsync();

    Task<GenericAPIResponse<IEnumerable<ModuleViewModel>>> GetUserModulesAsync(Guid userId);

    Task<GenericAPIResponse<IEnumerable<UsersModuleViewModel>>> GetUserModuleLinksAsync(Guid userId);

    Task<GenericAPIResponse<Guid?>> AddTopicAsync(Guid userId, Guid moduleId, CreateTopicRequest module, CancellationToken token);

    Task<GenericAPIResponse<IEnumerable<TopicViewModel>>> GetModuleTopicsAsync(Guid moduleId);

    Task<GenericAPIResponse<TopicViewModel>> GetModuleTopicAsync(Guid moduleId, Guid topicId);

    #region -- learing material section --

    Task<GenericAPIResponse<string>> AddLearningMaterialAsync(LearningMaterialViewModel model);

    #endregion -- learing material section --
}