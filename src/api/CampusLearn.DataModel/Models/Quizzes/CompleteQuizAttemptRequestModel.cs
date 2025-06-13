namespace CampusLearn.DataModel.Models.Quizzes;

public class CompleteQuizAttemptRequestModel
{
    public TimeSpan Duration { get; set; }

    public List<CreateQuizAttemptQuestionAnswerRequestModel> QuestionAnswers { get; set; }
}