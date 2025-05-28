namespace CampusLearn.DataModel.Models.Enquiries;

public record CreateEnquiryRequestModel
{
    public Guid ModuleId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}
