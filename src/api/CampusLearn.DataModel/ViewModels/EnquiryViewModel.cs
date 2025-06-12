namespace CampusLearn.DataModel.ViewModels;

public class EnquiryViewModel
{
    public Guid Id { get; set; }

    public Guid CreatedByUserId { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateResolved { get; set; }

    public Guid? ResolvedByUserId { get; set; }

    public EnquiryResolutionTypes? ResolutionAction { get; set; }

    public string? ResolutionResponse { get; set; }

    public EnquiryStatus Status { get; set; }

    public Guid ModuleId { get; set; }

    public Guid? LinkedTopicId { get; set; }

    public string? TopicTitle { get; set; }
}