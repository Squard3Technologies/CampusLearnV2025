namespace CampusLearn.DataModel.ViewModels;

public class QuizViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string ModuleCode { get; set; }

    public string TopicName { get; set; }

    public TimeSpan Duration { get; set; }
}
