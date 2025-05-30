using CampusLearn.DataModel.Models.Topic;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IModuleRepository
{
    Task<GenericDbResponseViewModel> AddModuleAsync(ModuleViewModel module);

    Task<GenericDbResponseViewModel> UpdateModuleAsync(ModuleViewModel module);

    Task<GenericDbResponseViewModel> ChangeModuleStatusAsync(Guid moduleId, bool status);

    Task<GenericDbResponseViewModel> AddUserModuleAsync(Guid userId, Guid moduleId);

    Task<GenericDbResponseViewModel> GetModulesAsync();

    Task<GenericDbResponseViewModel> GetUserModulesAsync(Guid userId);

    Task<GenericDbResponseViewModel<Guid?>> AddTopicAsync(Guid userId, CreateTopicRequest module);

    Task<GenericDbResponseViewModel> GetModuleTopicAsync(Guid moduleId);

    #region -- learing material section --
    Task<GenericDbResponseViewModel> AddLearningMaterialAsync(LearningMaterialViewModel model);

    #endregion


}
