namespace CampusLearnLib.Models;

public class QuizModel
{
    public Guid Id { get; set; }

    public Guid TopicId { get; set; }

    public string Title { get; set; }

    public IList<QuestionModel> Questions { get; set; }
}
