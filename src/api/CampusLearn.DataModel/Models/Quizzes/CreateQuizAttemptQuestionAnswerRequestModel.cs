namespace CampusLearn.DataModel.Models.Quizzes;

public class CreateQuizAttemptQuestionAnswerRequestModel
{
    public Guid QuestionId { get; set; }

    public Guid SelectedQuestionOptionId { get; set; }
}
