using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.User;
using CampusLearn.DataModel.ViewModels;
using CampusLearn.Services.Domain.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Users;

public class UserService : IUserService
{
    #region -- protected properties --
    protected readonly ILogger<UserService> logger;
    protected readonly IUserRepository userRepository;
    private readonly SecurePasswordHasher passwordHasher = new SecurePasswordHasher();
    private readonly JwtTokenProvider tokenProvider;
    #endregion

    public UserService(ILogger<UserService> logger, IUserRepository userRepository, JwtTokenProvider tokenProvider)
    {
        this.logger = logger;
        this.userRepository = userRepository;
        this.tokenProvider = tokenProvider;
    }



    public async Task<GenericAPIResponse<string>> LoginAsync(string emailAddress, string password)
    {
        GenericAPIResponse<string> response = new GenericAPIResponse<string>();

        var dbResponse = await userRepository.LoginAsync(emailAddress);
        if(dbResponse.Body != null)
        {
            bool verified = false;
            var user = (UserViewModel)dbResponse.Body;
            if (!string.IsNullOrEmpty(user?.Password))
            {
                verified = passwordHasher.Verify(plainPassword: password, passwordHash: user.Password);                
            }
            if (verified)
            {
                var token = tokenProvider.CreateToken(user);
                response = new GenericAPIResponse<string>()
                {
                    Status = true,
                    StatusCode = 200,
                    StatusMessage = $"Login successful",
                    Body = token,
                    StatusDetailedMessage = ""
                };
            }
            else
            {
                response = new GenericAPIResponse<string>()
                {
                    Status = true,
                    StatusCode = 404,
                    StatusMessage = $"Login failed. Incorrect plainPassword provided",
                    Body = "",
                    StatusDetailedMessage = ""
                };
            }
        }
        else
        {
            response = new GenericAPIResponse<string>()
            {
                Status = false,
                StatusCode = 404,
                StatusMessage = dbResponse.StatusMessage ?? $"Email address {emailAddress} is not registered",
                Body = "",
                StatusDetailedMessage = ""
            };
        }
        return response;
    }


    public async Task<GenericAPIResponse<CreateUserRequestModel>> CreateUserAccountAsync(CreateUserRequestModel user)
    {
        GenericAPIResponse<CreateUserRequestModel> response = new GenericAPIResponse<CreateUserRequestModel>();
        try
        {
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = passwordHasher.HashPassword(plainPassword: user.Password);
            }
            var dbResponse = await userRepository.CreateUserAccountAsync(user);
            if(dbResponse?.Status == true)
            {
                response = new GenericAPIResponse<CreateUserRequestModel>()
                {
                    Status = dbResponse.Status,
                    StatusCode = dbResponse.StatusCode,
                    StatusMessage = dbResponse.StatusMessage,
                    Body = user,
                    StatusDetailedMessage = ""
                };
            }
            else if (dbResponse?.Status == false)
            {
                response = new GenericAPIResponse<CreateUserRequestModel>()
                {
                    Status = dbResponse.Status,
                    StatusCode = dbResponse.StatusCode,
                    StatusMessage = dbResponse.StatusMessage,
                    Body = null,
                    StatusDetailedMessage = ""
                };
            }
            else
            {
                response = new GenericAPIResponse<CreateUserRequestModel>()
                {
                    Status = false,
                    StatusCode = 400,
                    StatusMessage = "Internal Database Service Error",
                    Body = null,
                    StatusDetailedMessage = ""
                };
            }
        }
        catch (Exception ex)
        {
            response = new GenericAPIResponse<CreateUserRequestModel>()
            {
                Status = false,
                StatusCode = 400,
                StatusMessage = $"{ex.Message} <br/> {ex.StackTrace}",
                Body = null,
                StatusDetailedMessage = ""
            };
        }
        return response;
    }

    public Task<GenericAPIResponse<string>> ChangeUserPasswordAsync(Guid userId, string password)
    {
        throw new NotImplementedException();
    }
}
