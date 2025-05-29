namespace CampusLearn.DataModel.Models.Quizzes;

public class CreateQuizAttemptRequestModel
{
    public List<CreateQuizAttemptQuestionAnswerRequestModel> QuestionAnswers { get; set; }
}
