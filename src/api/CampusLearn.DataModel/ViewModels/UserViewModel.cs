using CampusLearn.DataModel.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataModel.ViewModels;

public class UserViewModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string Surname { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public string ContactNumber { get; set; }
    public UserRoles Role { get; set; }
    public Guid AccountStatusId { get; set; }
    public string AccountStatusDescription { get; set; }
}
