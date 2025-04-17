namespace CampusLearnLib.ViewModels;

public abstract class User
{
    /// <summary>
    /// Internal system PK that we must keep in mind the amount of records it should store without reaching size limit.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Can store ID/Passport/StudentID
    /// </summary>
    public string ExternalId { get; set; }

    public string Name { get; set; }
    public string Surname { get; set; }
    public string EmailAddress { get; set; }
    string Password { get; set; }
    string ContactNumber { get; set; }


    public List<Message> messages { get; set; }

    public User()
    {
        messages = new List<Message>();
    }

    public void login()
    {
        Console.WriteLine($"UserModel {EmailAddress}logged in successfully.");
    }

    public void logout()
    {
        Console.WriteLine($"UserModel {EmailAddress}logged out successfully.");
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

        messages.Add(newMessage);
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
