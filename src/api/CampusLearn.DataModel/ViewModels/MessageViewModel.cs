using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.ViewModels;

public class MessageViewModel
{
    public Guid ID { get; set; }
    public Guid SenderID { get; set; }
    public string SenderName { get; set; }
    public string SenderSurname { get; set; }
    public string SenderEmailAddress { get; set; }
    public Guid ReceiverID { get; set; }
    public string ReceiverName { get; set; }
    public string ReceiverSurname { get; set; }
    public string ReceiverEmailAddress { get; set; }
    public string MessageBody { get; set; }
    public string MessageType { get; set; }
}
