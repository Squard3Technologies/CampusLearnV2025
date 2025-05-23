using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.User;

public class LoginRequestModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}
