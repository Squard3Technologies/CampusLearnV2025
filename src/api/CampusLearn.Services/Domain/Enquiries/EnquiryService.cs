using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models.Enquiries;
using CampusLearn.DataModel.Models.Enums;
using CampusLearn.DataModel.ViewModels;

namespace CampusLearn.Services.Domain.Enquiries;

public class EnquiryService : IEnquiryService
{
    private readonly IEnquiryRepository _enquiryRepository;

    public EnquiryService(IEnquiryRepository enquiryRepository)
    {
        _enquiryRepository = enquiryRepository;
    }

    public async Task CreateEnquiry(Guid userId, CreateEnquiryRequestModel model, CancellationToken token)
    {
        await _enquiryRepository.CreateEnquiryAsync(userId, model, token);
    }

    public async Task<List<EnquiryViewModel>> GetEnquiries(Guid userId, CancellationToken token)
    {
        return await _enquiryRepository.GetEnquiriesAsync(userId, token);
    }

    public Task<List<EnquiryViewModel>> GetEnquiriesByStatus(Guid userId, EnquiryStatus statusFilter, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task<EnquiryDetailViewModel> GetEnquiry(Guid userId, Guid id, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task ResolveEnquiry(Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}
