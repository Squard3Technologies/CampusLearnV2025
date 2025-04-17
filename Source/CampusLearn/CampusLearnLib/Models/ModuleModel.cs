using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib.Models;

public class ModuleModel
{
    public int Id { get; set; }

    public string UniqueCode { get; set; }

    public string Name { get; set; }

    public IList<TopicModel> Topics { get; set; } = new List<TopicModel>();
}
