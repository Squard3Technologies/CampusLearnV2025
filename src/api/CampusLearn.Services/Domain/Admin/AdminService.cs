using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.User;
using CampusLearn.DataModel.ViewModels;
using CampusLearn.Services.Domain.Users;
using CampusLearn.Services.Domain.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Admin;

public class AdminService : IAdminService
{
    #region -- protected properties --
    protected readonly ILogger<AdminService> logger;
    protected readonly IAdminRepository adminRepository;
    private readonly SecurePasswordHasher passwordHasher = new SecurePasswordHasher();
    private readonly JwtTokenProvider tokenProvider;
    #endregion

    public AdminService(ILogger<AdminService> logger, IAdminRepository adminRepository, JwtTokenProvider tokenProvider)
    {
        this.logger = logger;
        this.adminRepository = adminRepository;
        this.tokenProvider = tokenProvider;
    }

    public async Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetPendingRegistrationsAsync()
    {
        GenericAPIResponse<IEnumerable<UserViewModel>> apiResponse = new GenericAPIResponse<IEnumerable<UserViewModel>>();
        var dbResponse = await adminRepository.GetPendingRegistrationsAsync();
        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = (dbResponse.Body != null) ? (List<UserViewModel>)dbResponse.Body : null;
        return apiResponse;
    }


    public async Task<GenericAPIResponse<string>> ProcessRegistrationAsync(Guid userId, Guid accountStatusId)
    {
        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await adminRepository.ChangeUserAccountStatusAsync(userId, accountStatusId);
        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        return apiResponse;
    }


    public async Task<GenericAPIResponse<IEnumerable<UserViewModel>>> GetUsersAsync()
    {
        GenericAPIResponse<IEnumerable<UserViewModel>> apiResponse = new GenericAPIResponse<IEnumerable<UserViewModel>>();
        var dbResponse = await adminRepository.GetUsersAsync();
        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        apiResponse.Body = (dbResponse.Body != null) ? (List<UserViewModel>)dbResponse.Body : null;
        return apiResponse;
    }



    public async Task<GenericAPIResponse<string>> UpdateUserAsync(UserModel model)
    {
        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();

        var userViewModel = new UserViewModel()
        {
            Id = model.Id.Value,
            FirstName = model.FirstName,
            Surname = model.LastName,
            ContactNumber = model.ContactNumber,
            EmailAddress = model.EmailAddress,
        };

        var dbResponse = await adminRepository.UpdateUserAsync(userViewModel);
        apiResponse.Status = dbResponse.Status;
        apiResponse.StatusCode = dbResponse.StatusCode;
        apiResponse.StatusMessage = dbResponse.StatusMessage;
        return apiResponse;
    }

}
