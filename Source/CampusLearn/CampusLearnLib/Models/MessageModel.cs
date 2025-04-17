using CampusLearnLib.ViewModels;

namespace CampusLearnLib.Models;

public class MessageModel
{
    /// <summary>
    /// The ID of the user sending the message
    /// </summary>
    public Guid SenderId { get; set; }

    /// <summary>
    /// The ID of the message recipient
    /// </summary>
    public Guid ReceiverId { get; set; }

    /// <summary>
    /// The message detail
    /// </summary>
    public string ContentBody { get; set; }

    /// <summary>
    /// The date the message was created
    /// </summary>
    public DateTime DateCreated { get; set; }

    public UserModel Receiver { get; set; }

    public UserModel Sender { get; set; }
}
