namespace CampusLearn.DataModel.ViewModels;

public class QuizAttemptViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<QuizAttemptQuestionViewModel> Questions { get; set; }
}
