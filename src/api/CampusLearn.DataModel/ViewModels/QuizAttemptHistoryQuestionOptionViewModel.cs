namespace CampusLearn.DataModel.ViewModels;

public class QuizAttemptHistoryQuestionOptionViewModel
{
    public Guid Id { get; set; }

    public Guid QuestionId { get; set; }

    public string Name { get; set; }

    public bool IsCorrect { get; set; }

    public bool IsChosen { get; set; }
}
