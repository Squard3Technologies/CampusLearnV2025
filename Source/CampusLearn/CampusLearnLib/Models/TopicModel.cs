namespace CampusLearnLib.Models;

public class TopicModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public TutorModel CreatedBy { get; set; }

    public IList<MaterialModel> Materials { get; set; } = new List<MaterialModel>();

    /// <summary>
    /// All user who commented that has a role type: student
    /// </summary>
    public IList<StudentModel> Subscribers { get; set; } = new List<StudentModel>();

    public IList<DiscussionModel> Discussions { get; set; } = new List<DiscussionModel>();

    public IList<QuizModel> Quizzes { get; set; } = new List<QuizModel>();

    public DateTime DateCreated { get; set; }
}