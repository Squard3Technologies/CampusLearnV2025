using CampusLearn.DataModel.Models.Modules;
using CampusLearn.DataModel.Models.Topic;
using CampusLearn.DataModel.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;

namespace CampusLearn.Services.Domain.Modules;

public class ModuleService : IModuleService
{
    #region -- protected properties --

    protected readonly ILogger<ModuleService> logger;
    protected readonly IModuleRepository moduleRepository;
    private readonly INotificationService _notificationService;

    #endregion -- protected properties --

    public ModuleService(ILogger<ModuleService> logger,
        IModuleRepository moduleRepository,
        INotificationService notificationService)
    {
        this.logger = logger;
        this.moduleRepository = moduleRepository;
        _notificationService = notificationService;
    }

    public async Task<GenericAPIResponse<string>> AddModuleAsync(CreateModuleRequest module)
    {
        ModuleViewModel moduleViewModel = new ModuleViewModel()
        {
            Id = Guid.NewGuid(),
            Name = module.Name,
            Code = module.Code,
        };

        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await moduleRepository.AddModuleAsync(moduleViewModel);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;

        return apiResponse;
    }

    public async Task<GenericAPIResponse<string>> UpdateModuleAsync(CreateModuleRequest module)
    {
        ModuleViewModel moduleViewModel = new ModuleViewModel()
        {
            Id = module.Id.Value,
            Name = module.Name,
            Code = module.Code,
        };

        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await moduleRepository.UpdateModuleAsync(moduleViewModel);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;

        return apiResponse;
    }

    public async Task<GenericAPIResponse<string>> ChangeModuleStatusAsync(Guid moduleId, bool status)
    {
        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await moduleRepository.ChangeModuleStatusAsync(moduleId, status);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        return apiResponse;
    }

    public async Task<GenericAPIResponse<string>> AddUserModuleAsync(Guid userId, Guid moduleId)
    {
        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await moduleRepository.AddUserModuleAsync(userId, moduleId);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        return apiResponse;
    }

    public async Task<GenericAPIResponse<IEnumerable<ModuleViewModel>>> GetModulesAsync()
    {
        GenericAPIResponse<IEnumerable<ModuleViewModel>> apiResponse = new GenericAPIResponse<IEnumerable<ModuleViewModel>>();
        var dbResponse = await moduleRepository.GetModulesAsync();

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = (dbResponse.Body != null) ? (List<ModuleViewModel>)dbResponse.Body : null;
        return apiResponse;
    }

    public async Task<GenericAPIResponse<IEnumerable<UsersModuleViewModel>>> GetUserModulesAsync(Guid userId)
    {
        GenericAPIResponse<IEnumerable<UsersModuleViewModel>> apiResponse = new GenericAPIResponse<IEnumerable<UsersModuleViewModel>>();
        var dbResponse = await moduleRepository.GetUserModulesAsync(userId);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = (dbResponse.Body != null) ? (List<UsersModuleViewModel>)dbResponse.Body : null;
        return apiResponse;
    }

    public async Task<GenericAPIResponse<Guid?>> AddTopicAsync(Guid userId, CreateTopicRequest model, CancellationToken token)
    {
        GenericAPIResponse<Guid?> apiResponse = new GenericAPIResponse<Guid?>();
        var dbResponse = await moduleRepository.AddTopicAsync(userId, model);
        if (dbResponse.Body != null)
            await _notificationService.SendTopicCreatedAsync(userId, dbResponse.Body.Value, NotificationTypes.Email, token);
        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = dbResponse.Body;
        return apiResponse;
    }

    public async Task<GenericAPIResponse<IEnumerable<TopicViewModel>>> GetModuleTopicAsync(Guid moduleId)
    {
        GenericAPIResponse<IEnumerable<TopicViewModel>> apiResponse = new GenericAPIResponse<IEnumerable<TopicViewModel>>();
        var dbResponse = await moduleRepository.GetModuleTopicAsync(moduleId);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = (dbResponse.Body != null) ? (List<TopicViewModel>)dbResponse.Body : null;
        return apiResponse;
    }




    #region -- learing material section --

    public async Task<GenericAPIResponse<string>> AddLearningMaterialAsync(LearningMaterialViewModel model)
    {
        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await moduleRepository.AddLearningMaterialAsync(model);

        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = (dbResponse.Status) ? model.FilePath : "";
        return apiResponse;
    }


    public async Task<GenericDbResponseViewModel> GetUserLearningMaterialAsync(Guid userId)
    {
        GenericDbResponseViewModel response = new GenericDbResponseViewModel();
        
        return response;
    }


    public async Task<GenericDbResponseViewModel> GetUserTopicLearningMaterialAsync(Guid userId, Guid topicId)
    {
        GenericDbResponseViewModel response = new GenericDbResponseViewModel();
        
        return response;
    }


    public async Task<GenericDbResponseViewModel> GetTopicLearningMaterialAsync(Guid topicId)
    {
        GenericDbResponseViewModel response = new GenericDbResponseViewModel();
        
        return response;
    }

    #endregion -- topic section --


}
