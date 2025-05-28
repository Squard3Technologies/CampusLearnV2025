namespace CampusLearn.DataModel.Models.Enquiries;

public record ResolveEnquiryRequestModel
{
    public EnquiryResolutionTypes ResolutionAction { get; set; }

    public string ResolutionResponse { get; set; }

    public Guid? LinkedTopicId { get; set; }
}
