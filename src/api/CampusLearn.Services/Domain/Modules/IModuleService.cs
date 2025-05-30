using CampusLearn.DataModel.Models.Modules;
using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.Services.Domain.Modules;

public interface IModuleService
{
    Task<GenericAPIResponse<string>> AddModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> UpdateModuleAsync(CreateModuleRequest module);

    Task<GenericAPIResponse<string>> ChangeModuleStatusAsync(Guid moduleId, bool status);

    Task<GenericAPIResponse<string>> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericAPIResponse<IEnumerable<ModuleViewModel>>> GetModulesAsync();

    Task<GenericAPIResponse<IEnumerable<UsersModuleViewModel>>> GetUserModulesAsync(Guid userId);

    Task<GenericAPIResponse<Guid?>> AddTopicAsync(Guid userId, CreateTopicRequest module, CancellationToken token);

    Task<GenericAPIResponse<IEnumerable<TopicViewModel>>> GetModuleTopicAsync(Guid moduleId);
}
