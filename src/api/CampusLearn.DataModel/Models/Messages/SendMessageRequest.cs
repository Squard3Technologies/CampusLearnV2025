namespace CampusLearn.DataModel.Models.Messages;

public class SendMessageRequest
{
    public Guid? SenderId { get; set; }
    public Guid RecieverId { get; set; }
    public string MessageContent { get; set; }
}
