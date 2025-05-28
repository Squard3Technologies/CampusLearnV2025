namespace CampusLearn.DataModel.ViewModels;

public class ChatMessageViewModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string UserName { get; set; }

    public string UserSurname { get; set; }

    public string UserEmail { get; set; }

    public string Content { get; set; }

    public DateTime DateCreated { get; set; }
}
