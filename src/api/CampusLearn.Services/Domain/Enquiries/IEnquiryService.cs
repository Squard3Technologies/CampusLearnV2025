using CampusLearn.DataModel.Models.Enquiries;

namespace CampusLearn.Services.Domain.Enquiries;

public interface IEnquiryService
{
    Task<List<EnquiryViewModel>> GetEnquiries(Guid userId, CancellationToken token);

    Task CreateEnquiry(Guid userId, CreateEnquiryRequestModel model, CancellationToken token);

    Task<EnquiryViewModel?> GetEnquiry(Guid userId, Guid id, CancellationToken token);

    Task<List<EnquiryViewModel>> GetEnquiriesByStatus(EnquiryStatus statusFilter, CancellationToken token);

    Task ResolveEnquiry(Guid enquiryId, Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token);
}
