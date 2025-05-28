namespace CampusLearn.DataModel.ViewModels;

public class QuizQuestionViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public QuestionTypes QuestionType { get; set; }

    public List<QuizQuestionOptionViewModel> QuestionOptions { get; set; }
}
