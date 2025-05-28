namespace CampusLearn.DataModel.Models.Quizzes;

public class CreateQuizQuestionRequestModel
{
    public string Name { get; set; }

    public QuestionTypes QuestionType { get; set; }

    public List<CreateQuizQuestionOptionRequestModel> QuestionOptions { get; set; }
}
