namespace CampusLearn.DataModel.ViewModels;

public class QuizHistoryViewModel
{
    public Guid Id { get; set; }

    public string QuizName { get; set; }

    public string ModuleCode { get; set; }

    public string TopicName { get; set; }

    public string Duration { get; set; }

    public DateTime LastAttemptDateTime { get; set; }

    public string LastAttemptScore { get; set; }
}
