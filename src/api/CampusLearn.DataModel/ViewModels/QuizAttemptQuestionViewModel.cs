namespace CampusLearn.DataModel.ViewModels;

public class QuizAttemptQuestionViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public List<QuizAttemptQuestionOptionViewModel> QuestionOptions { get; set; }
}
