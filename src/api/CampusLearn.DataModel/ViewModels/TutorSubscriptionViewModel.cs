namespace CampusLearn.DataModel.ViewModels;

public class TutorSubscriptionViewModel
{
    public Guid TutorId { get; set; }

    public string Name { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public List<ModuleViewModel> LinkedModules { get; set; }
}
