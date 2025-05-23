using CampusLearn.DataModel.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.Models.User;

public class CreateUserRequestModel
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string ContactNumber { get; set; }
    public UserRoles Role { get; set; }
}
