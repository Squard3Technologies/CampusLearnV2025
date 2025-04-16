namespace CampusLearnLib
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        string Password { get; set; }
        string ContactNumber { get; set; }
        public List<Message> messages { get; set; }

        public User()
        {
            messages = new List<Message>();
        }

        public void login()
        {
            Console.WriteLine($"User {Email}logged in successfully.");
        }

        public void logout()
        {
            Console.WriteLine($"User {Email}logged out successfully.");
        }

        public void sendMessage(User receiver, string context)
        {
            Message newMessage = new Message
            {
                sender = this,
                receiver = receiver,
                content = context,
                timestamp = DateTime.Now
            };

            this.messages.Add(newMessage);
            receiver.messages.Add(newMessage);
            Console.WriteLine($"Message sent to {receiver.Name}{receiver.Surname}");
        }

        public List<Message> GetMessagesHistory()
        {
            return messages;
        }
    }
    public class Message
    {
        public User sender { get; set; }
        public User receiver { get; set; }
        public string content { get; set; }
        public DateTime timestamp { get; set; }
    }
}
