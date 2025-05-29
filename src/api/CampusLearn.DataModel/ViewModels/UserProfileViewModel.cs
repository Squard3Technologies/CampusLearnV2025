namespace CampusLearn.DataModel.ViewModels;

public class UserProfileViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public string ContactNumber { get; set; }

    public List<UserProfileModuleViewModel> LinkedModules { get; set; }
}
