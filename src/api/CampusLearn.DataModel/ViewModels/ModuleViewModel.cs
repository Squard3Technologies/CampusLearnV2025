using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CampusLearn.DataModel.ViewModels;

public class ModuleViewModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}



public class TopicViewModel
{
    public Guid Id { get; set; }
    public Guid CreatedByUserId { get; set; }
    public Guid ModuleId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
}
