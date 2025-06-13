namespace CampusLearn.Services.Domain.Admin;

public class AdminService : IAdminService
{
    #region -- protected properties --

    protected readonly ILogger<AdminService> logger;
    protected readonly IAdminRepository adminRepository;
    private readonly SecurePasswordHasher passwordHasher = new SecurePasswordHasher();
    private readonly JwtTokenProvider tokenProvider;
    private readonly INotificationService _notificationService;

    #endregion -- protected properties --

    public AdminService(ILogger<AdminService> logger,
        IAdminRepository adminRepository,
        JwtTokenProvider tokenProvider,
        INotificationService notificationService)
    {
        this.logger = logger;
        this.adminRepository = adminRepository;
        this.tokenProvider = tokenProvider;
        _notificationService = notificationService;
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

    public async Task<GenericAPIResponse<string>> ProcessRegistrationAsync(Guid userId, Guid accountStatusId, CancellationToken token)
    {
        GenericAPIResponse<string> apiResponse = new GenericAPIResponse<string>();
        var dbResponse = await adminRepository.ChangeUserAccountStatusAsync(userId, accountStatusId);

        if (dbResponse.Status)
        {
            if (accountStatusId == AccountStatus.ACTIVE)
                await _notificationService.SendAccountApprovedAsync(userId, NotificationTypes.Email, token);

            if (accountStatusId == AccountStatus.INACTIVE)
                await _notificationService.SendAccountDeactivatedAsync(userId, NotificationTypes.Email, token);

            if (accountStatusId == AccountStatus.REJECTED)
                await _notificationService.SendAccountRejectedAsync(userId, NotificationTypes.Email, token);
        }

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
            MiddleName = model.MiddleName,
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
