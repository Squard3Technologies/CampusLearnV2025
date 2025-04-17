namespace CampusLearnLib.Models;

public class CommentModel
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid TopicId { get; set; }
    public string ContentBody { get; set; }
    public DateTime DateCreated { get; set; }
}

