namespace CampusLearn.DataModel.ViewModels;

public class QuizAttemptHistoryViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<QuizAttemptHistoryQuestionViewModel> Questions { get; set; }
}
