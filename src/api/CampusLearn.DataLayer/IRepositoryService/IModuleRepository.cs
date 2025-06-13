using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IModuleRepository
{
    Task<GenericDbResponseViewModel<Guid?>> AddModuleAsync(ModuleViewModel module);

    Task<GenericDbResponseViewModel> UpdateModuleAsync(ModuleViewModel module);

    Task<GenericDbResponseViewModel> ChangeModuleStatusAsync(Guid moduleId, bool status);

    Task<GenericDbResponseViewModel> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericDbResponseViewModel> GetModulesAsync();

    Task<GenericDbResponseViewModel> GetUserModulesAsync(Guid userId);

    Task<GenericDbResponseViewModel> GetUserModuleLinksAsync(Guid userId);

    Task<GenericDbResponseViewModel<Guid?>> AddTopicAsync(Guid userId, Guid moduleId, CreateTopicRequest module);

    Task<GenericDbResponseViewModel> GetModuleTopicsAsync(Guid moduleId);

    Task<GenericDbResponseViewModel> GetModuleTopicAsync(Guid moduleId, Guid topicId);

    #region -- learing material section --

    Task<GenericDbResponseViewModel> AddLearningMaterialAsync(LearningMaterialViewModel model);

    #endregion -- learing material section --
}