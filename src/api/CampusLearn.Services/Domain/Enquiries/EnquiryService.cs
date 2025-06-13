using CampusLearn.DataModel.Models.Enquiries;

namespace CampusLearn.Services.Domain.Enquiries;

public class EnquiryService : IEnquiryService
{
    private readonly IEnquiryRepository _enquiryRepository;
    private readonly IModuleRepository _moduleRepository;
    private readonly INotificationService _notificationService;

    public EnquiryService(IEnquiryRepository enquiryRepository,
        IModuleRepository moduleRepository,
        INotificationService notificationService)
    {
        _enquiryRepository = enquiryRepository;
        _moduleRepository = moduleRepository;
        _notificationService = notificationService;
    }

    public async Task CreateEnquiry(Guid userId, CreateEnquiryRequestModel model, CancellationToken token)
    {
        var enquiryId = await _enquiryRepository.CreateEnquiryAsync(userId, model, token);
        if (enquiryId != null)
            await _notificationService.SendEnquiryCreatedAsync(userId, enquiryId.Value, NotificationTypes.Email, token);
    }

    public async Task<List<EnquiryViewModel>> GetEnquiries(Guid userId, CancellationToken token)
    {
        return await _enquiryRepository.GetEnquiriesAsync(userId, token);
    }

    public async Task<List<EnquiryViewModel>> GetEnquiriesByStatus(EnquiryStatus statusFilter, CancellationToken token)
    {
        return await _enquiryRepository.GetEnquiriesByStatusAsync(statusFilter, token);
    }

    public async Task<EnquiryViewModel?> GetEnquiry(Guid userId, Guid id, CancellationToken token)
    {
        return await _enquiryRepository.GetEnquiryAsync(id, token);
    }

    public async Task ResolveEnquiry(Guid enquiryId, Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token)
    {
        var enquiry = await GetEnquiry(tutorId, enquiryId, token);

        if (model.ResolutionAction == EnquiryResolutionTypes.CreateNewTopic)
        {
            model.LinkedTopicId = (await _moduleRepository.AddTopicAsync(tutorId, enquiry.ModuleId, new DataModel.Models.Topic.CreateTopicRequest()
            {
                Title = enquiry.Title,
                Description = enquiry.Description
            })).Body;
        }

        await _enquiryRepository.ResolveEnquiryAsync(enquiryId, tutorId, model, token);

        await _notificationService.SendEnquiryResolvedAsync(tutorId, enquiryId, NotificationTypes.Email, token);
    }
}