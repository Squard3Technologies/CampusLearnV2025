namespace CampusLearn.DataModel.ViewModels;


public class GenericDbResponseViewModel
{
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string StatusMessage { get; set; }
    public object? Body { get; set; }
}
