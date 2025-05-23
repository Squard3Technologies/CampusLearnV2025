namespace CampusLearn.DataModel.Models;

public class GenericAPIResponse<T>
{
    public bool Status { get; set; }
    public int StatusCode { get; set; }
    public string StatusMessage { get; set; }
    public string StatusDetailedMessage { get; set; }
    public T Body { get; set; }
}
