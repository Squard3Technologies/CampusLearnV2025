namespace CampusLearn.Services.Domain.Utils;

public class ScheduleManager
{
    private readonly ScheduleSetting _scheduleSettings;
    private readonly ILogger<ScheduleManager> _logger;
    private readonly IServiceProvider _serviceProvider;
    public static IDictionary<string, ScheduleSetting> customSchedules = new Dictionary<string, ScheduleSetting>();

    public ScheduleManager(ILogger<ScheduleManager> logger, IOptions<ScheduleSetting> options, IServiceProvider serviceProvider)
    {
        _scheduleSettings = options.Value;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    #region -- public schedules functions --

    /// <summary>
    /// Starting all available and configured customSchedules
    /// </summary>
    /// <returns></returns>
    public Task ConfigureSchedulesAsync(bool autoStart = false)
    {
        _logger.LogInformation($"Start {this}");
        return Task.Run(() =>
        {
            var schedule = _scheduleSettings;
            //foreach (var schedule in scheduleSettings)
            {
                _logger.LogInformation($"************************************");
                _logger.LogInformation($"Schedule Name: {schedule.Name}");
                _logger.LogInformation($"Schedule Hour: {schedule.Hour}");
                _logger.LogInformation($"Schedule Minutes: {schedule.Minutes}");
                _logger.LogInformation($"Schedule Interval: {schedule.Interval}");
                _logger.LogInformation($"Schedule Active: {schedule.Active}");
                _logger.LogInformation($"************************************");
                _logger.LogInformation($"{Environment.NewLine}");

                Action action = () => { };
                if (schedule.ID.ToUpper() == "3696E8A9-6AE9-40C1-A2A0-83201A0CE3B8" || schedule.Name.ToUpper() == "EMIALING")
                {
                    action = async () =>
                    {
                        var notificationService = _serviceProvider.GetRequiredService<INotificationService>();
                        if (!notificationService.EMAILServiceBusy())
                        {
                            await notificationService.StartEmailingServiceAsync();
                        }
                    };
                }
                ScheduleBasicTaskV2(schedule: schedule, task: action, autoStart: autoStart);
            }
        });
    }

    /// <summary>
    /// Stopping all running customSchedules.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> StopSchedulesAsync()
    {
        return await Task.Run(async () =>
        {
            if (customSchedules.Count() < 1)
                return false;

            foreach (var timer in customSchedules)
            {
                customSchedules[timer.Key].timer.Stop();
            }
            return !customSchedules.Any(x => x.Value.timer.Enabled);
        });
    }

    /// <summary>
    /// Starting all available customSchedules.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> StartSchedulesAsync()
    {
        return await Task.Run(async () =>
        {
            if (customSchedules.Count() < 1)
                return false;

            foreach (var timer in customSchedules)
            {
                customSchedules[timer.Key].timer.Start();
            }
            return true;
        });
    }

    #endregion -- public schedules functions --

    #region -- protected schedules functions --

    private void ScheduleTaskV2(ScheduleSetting schedule, Action task, bool autoStart = false)
    {
        if (!customSchedules.ContainsKey(schedule.ID))
        {
            DateTime now = DateTime.Now;
            DateTime firstTimeRun = new DateTime(now.Year, now.Month, now.Day, schedule.Hour, schedule.Minutes, 0, 0);
            if (now > firstTimeRun)
            {
                firstTimeRun = firstTimeRun.AddMinutes(1);
            }

            TimeSpan timeToGo = firstTimeRun - now;
            //removing configuration to allow immediate running of the schedule.
            //if (timeToGo < TimeSpan.Zero)
            {
                timeToGo = TimeSpan.Zero;
            }

            double intervalInMilliseconds = ((schedule.Interval * 60) * 1000);

            #region -- using System.Timers.Timer --

            var timerV2 = new System.Timers.Timer();
            timerV2.Interval = intervalInMilliseconds;
            timerV2.Elapsed += (s, e) => { task.Invoke(); };
            schedule.timer = timerV2;

            if (!string.IsNullOrEmpty(schedule.ID))
            {
                customSchedules[schedule.ID.ToString().ToUpper()] = schedule;
                if (autoStart && schedule.Active)
                {
                    customSchedules[schedule.ID.ToString().ToUpper()].timer.Start();
                }
            }
            else
            {
                customSchedules[Guid.NewGuid().ToString().ToUpper()] = schedule;
                if (autoStart && schedule.Active)
                {
                    customSchedules[schedule.ID.ToString().ToUpper()].timer.Start();
                }
            }

            #endregion -- using System.Timers.Timer --
        }
        else
        {
            if (autoStart)
            {
                customSchedules[schedule.ID].timer.Start();
            }
        }
    }

    private void ScheduleBasicTaskV2(ScheduleSetting schedule, Action task, bool autoStart = false)
    {
        ScheduleTaskV2(schedule, task, autoStart);
    }

    #endregion -- protected schedules functions --
}