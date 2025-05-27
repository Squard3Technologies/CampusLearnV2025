namespace CampusLearn.DataModel.Models.Enquiries;

public record ResolveEnquiryRequestModel
{
    public Guid ModuleId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }
}
