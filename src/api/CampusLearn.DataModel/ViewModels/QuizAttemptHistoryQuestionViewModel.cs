namespace CampusLearn.DataModel.ViewModels;

public class QuizAttemptHistoryQuestionViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public QuestionTypes QuestionType { get; set; }

    public List<QuizAttemptHistoryQuestionOptionViewModel> QuestionOptions { get; set; }
}
