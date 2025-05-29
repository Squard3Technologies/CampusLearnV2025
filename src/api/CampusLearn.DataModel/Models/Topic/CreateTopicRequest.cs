using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.Topic;

public class CreateTopicRequest
{
    public Guid ModuleId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}