namespace CampusLearn.DataModel.Models.Quizzes;

public class CreateQuizQuestionOptionRequestModel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Value { get; set; }

    public bool IsCorrect { get; set; }
}
