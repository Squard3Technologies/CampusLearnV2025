using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.ViewModels;

public class UserModuleViewModel : UserViewModel
{
    public Guid ModuleId { get; set; }
    public string ModuleCode { get; set; }
    public string ModuleName { get; set; }
}
