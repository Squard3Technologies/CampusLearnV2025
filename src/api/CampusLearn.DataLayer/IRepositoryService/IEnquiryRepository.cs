namespace CampusLearn.DataLayer.IRepositoryService;

public interface IEnquiryRepository
{
    Task<List<EnquiryViewModel>> GetEnquiriesAsync(Guid userId, CancellationToken token);

    Task CreateEnquiryAsync(Guid userId, CreateEnquiryRequestModel model, CancellationToken token);

    Task<GenericDbResponseViewModel> GetEnquiryAsync(Guid userId, Guid id, CancellationToken token);

    Task<GenericDbResponseViewModel> GetEnquiriesByStatus(Guid userId, EnquiryStatus statusFilter, CancellationToken token);

    Task ResolveEnquiry(Guid tutorId, ResolveEnquiryRequestModel model, CancellationToken token);
}
