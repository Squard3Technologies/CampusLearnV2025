using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.Messages;

public class SendMessageRequest
{
    public Guid SenderId { get; set; }
    public Guid RecieverId { get; set; }
    public string MessageContent { get; set; }
}
