namespace CampusLearn.DataModel.ViewModels;

public class DiscussionCommentViewModel
{
    public Guid Id { get; set; }

    public string Content { get; set; }

    public DateTime DateCreated { get; set; }

    public Guid CreatedByUserId { get; set; }

    public string CreatedByUserName { get; set; }

    public string CreatedByUserSurname { get; set; }

    public string CreatedByUserEmail { get; set; }
}
