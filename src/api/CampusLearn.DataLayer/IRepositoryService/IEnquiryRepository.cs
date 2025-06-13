namespace CampusLearn.DataLayer.IRepositoryService;

public interface IEnquiryRepository
{
    Task<List<EnquiryViewModel>> GetEnquiriesAsync(Guid userId, CancellationToken token);

    Task<Guid?> CreateEnquiryAsync(Guid userId, CreateEnquiryRequestModel model, CancellationToken token);

    Task<EnquiryViewModel?> GetEnquiryAsync(Guid id, CancellationToken token);

    Task<List<EnquiryViewModel>> GetEnquiriesByStatusAsync(EnquiryStatus statusFilter, CancellationToken token);

    Task<GenericDbResponseViewModel> ResolveEnquiryAsync(Guid enquiryId, Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token);
}