namespace CampusLearnLib.ViewModels;

internal class Tutors : User
{
    public List<Topic> ownedTopics { get; set; }

    public Tutors() : base()
    {
        ownedTopics = new List<Topic>();
    }

    public Topic createTopic(string title, string description)
    {
        Topic newTopic = new Topic
        {
            Id = GenerateTopicId(),
            Name = title,
            Description = description,
            owner = this
        };
        ownedTopics.Add(newTopic);

        Console.WriteLine($"Created new topic: {title}");
        return newTopic;
    }

    public void uploadMaterial(Material material)
    {
        Topic topic = ownedTopics.FirstOrDefault(t => t.id == material.topicId);
        if (topic != null)
        {
            topic.materials.Add(material);
            Console.WriteLine($"Material {material.title} uploaded to topic: {topic.name}");
        }
        else
        {
            Console.WriteLine("error message");
        }
    }
    public List<Quiz> studentQuizHistory(Student student)
    {
        Console.WriteLine($"Getting quiz history from student: {student.Name}{student.Surname} ");

        List<Quiz> relevantQuizzes = student.completedQuizzes.Where(quiz => ownedTopics.Any(topic => topic.quizzes.Contains(quiz))).ToList();
        return relevantQuizzes;
    }

    private int GenerateTopicId()
    {
        Random rand = new Random();
        return rand.Next(1000, 9999);
    }
}

public class Material
{
    public int id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string fileUrl { get; set; }
    public int topicId { get; set; }
    public DateTime uploadDate { get; set; }
}

public partial class Topic
{
    public Tutor owner { get; set; }
    public List<Material> materials { get; set; } = new List<Material>();
    public List<Quiz> quizzes { get; set; } = new List<Quiz>();
}


}
