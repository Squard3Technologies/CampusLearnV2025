using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib.Models;

public class TutorModel : UserModel
{
    public string EmployeeNo { get; set; }

    public TutorModel(): base() { }
}


