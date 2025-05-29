using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.Modules;

public class CreateModuleRequest
{
    public Guid? Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
}


public class CreateUserModuleRequest
{
    public Guid UserId { get; set; }
    public Guid ModuleId { get; set; }
}
