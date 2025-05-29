using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.Modules;
using CampusLearn.Services.Domain.Admin;
using CampusLearn.Services.Domain.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Modules;

public class ModuleService : IModuleService
{
    #region -- protected properties --
    protected readonly ILogger<ModuleService> logger;
    protected readonly IModuleRepository moduleRepository;
    #endregion

    public ModuleService(ILogger<ModuleService> logger, IModuleRepository moduleRepository)
    {
        this.logger = logger;
        this.moduleRepository = moduleRepository;        
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
}
