using Timer = System.Timers.Timer;
namespace CampusLearn.DataModel.Models.Configurations;

public class ScheduleSetting
{
    public string ID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Hour { get; set; }
    public int Minutes { get; set; }
    public int Interval { get; set; }
    public bool Active { get; set; }
    public Timer timer { get; set; }
}
