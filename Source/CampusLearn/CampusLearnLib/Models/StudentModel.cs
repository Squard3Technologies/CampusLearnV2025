using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearnLib.Models;

public class StudentModel : UserModel
{
    public string StudentNo { get; set; }

    public StudentModel() : base() { }

}
