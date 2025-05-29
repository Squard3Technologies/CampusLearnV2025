using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.User;

public class ChangeAccountRequest
{
    public Guid UserId { get; set; }
    public Guid AccountStatusId { get; set; }
}
