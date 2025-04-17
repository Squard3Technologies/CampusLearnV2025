using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib.ViewModels
{
    public class Student : User
    {
        public List<Topic> subscribedTopics { get; set; }
        public List<Tutor> subscribedTutors { get; set; }
        public List<Quiz> completedQuizzes { get; set; }

        public Student() : base()
        {
            subscribedTopics = new List<Topic>();
            subscribedTutors = new List<Tutor>();
            completedQuizzes = new List<Quiz>();
        }

        public void subscribeToTopic(Topic topic)
        {
            subscribedTopics.Add(topic);
            Console.WriteLine($"Subscribed to topic: {topic.name}");

        }
        public void createDiscussion(Topic topic)
        {
            Console.WriteLine($"Created a new discussion in topic: {topic.name}");

        }

        public void attemptQuiz(Quiz quiz)
        {
            Console.WriteLine($"Attempting quiz: {quiz.title}");

            completedQuizzes.Add(quiz);
            Console.WriteLine("The quiz is completed and has been added to your list of completed quizzes");

        }
        public void register()
        {
            Console.WriteLine($"Student{Name}{Surname} registered successfully");
        }

        public class Topic
        {
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }

        public class Tutor
        {
            public int id { get; set; }
            public string name { get; set; }
        }
        public class Quiz
        {
            public int id { get; set; }
            public string title { get; set; }
        }
    }
}
