namespace CampusLearnLib.Models;

public class DiscussionModel
{
    public Guid Id { get; set; }
    public Guid TopicId { get; set; }
    public string Title { get; set; }
    public IList<CommentModel> Comments { get; set; }
}
