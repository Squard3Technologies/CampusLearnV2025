using CampusLearn.DataModel.Models.Enquiries;
using CampusLearn.DataModel.Models.Enums;
using CampusLearn.DataModel.ViewModels;

namespace CampusLearn.Services.Domain.Enquiries;

public interface IEnquiryService
{
    Task<List<EnquiryViewModel>> GetEnquiries(Guid userId, CancellationToken token);

    Task CreateEnquiry(Guid userId, CreateEnquiryRequestModel model, CancellationToken token);

    Task<EnquiryDetailViewModel> GetEnquiry(Guid userId, Guid id, CancellationToken token);

    Task<List<EnquiryViewModel>> GetEnquiriesByStatus(Guid userId, EnquiryStatus statusFilter, CancellationToken token);

    Task ResolveEnquiry(Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token);
}
