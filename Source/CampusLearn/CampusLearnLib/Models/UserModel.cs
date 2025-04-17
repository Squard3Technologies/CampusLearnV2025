namespace CampusLearnLib.Models;

public abstract class UserModel
{
    /// <summary>
    /// Internal system PK that we must keep in mind the amount of records it should store without reaching size limit.
    /// </summary>
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string EmailAddress { get; set; }

    string Password { get; set; }

    string ContactNumber { get; set; }

    /// <summary>
    /// Student | Admin | Tutor | Lecturer
    /// </summary>
    public Guid RoleId { get; set; }

    public string RoleDescription { get; set; }

    public List<MessageModel> Messages { get; set; }

    public UserModel()
    {
        Messages = new List<MessageModel>();
    }

}
