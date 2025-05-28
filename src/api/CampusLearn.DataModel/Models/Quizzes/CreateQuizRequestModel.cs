namespace CampusLearn.DataModel.Models.Quizzes;

public class CreateQuizRequestModel
{
    public string Name { get; set; }

    public string Description { get; set; }

    public Guid TopicId { get; set; }

    public TimeSpan Duration { get; set; }
}
