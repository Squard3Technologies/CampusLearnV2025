using CampusLearn.DataModel.Models.Configurations;
using CampusLearn.Services.Domain.Emailing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Utils;

public class ScheduleManager
{
    #region -- protected properties --
    protected readonly ILogger<ScheduleManager> logger;
    protected readonly ScheduleSetting scheduleSettings;
    protected readonly IMessagingServices emailingServices;
    public static IDictionary<string, ScheduleSetting> customSchedules = new Dictionary<string, ScheduleSetting>();
    #endregion


    public ScheduleManager(ILogger<ScheduleManager> logger, IOptions<ScheduleSetting> options, IMessagingServices emailingServices)
    {
        this.logger = logger;
        this.scheduleSettings = options.Value;
        this.emailingServices = emailingServices;
    }



    #region -- public schedules functions --

    /// <summary>
    /// Starting all available and configured customSchedules
    /// </summary>
    /// <returns></returns>
    public Task ConfigureSchedulesAsync(bool autoStart = false)
    {
        logger.LogInformation($"Start {this}");
        return Task.Run(() =>
        {
            var schedule = scheduleSettings;
            //foreach (var schedule in scheduleSettings)
            {
                logger.LogInformation($"************************************");
                logger.LogInformation($"Schedule Name: {schedule.Name}");
                logger.LogInformation($"Schedule Hour: {schedule.Hour}");
                logger.LogInformation($"Schedule Minutes: {schedule.Minutes}");
                logger.LogInformation($"Schedule Interval: {schedule.Interval}");
                logger.LogInformation($"Schedule Active: {schedule.Active}");
                logger.LogInformation($"************************************");
                logger.LogInformation($"{Environment.NewLine}");

                Action action = () => { };
                if (schedule.ID.ToUpper() == "3696E8A9-6AE9-40C1-A2A0-83201A0CE3B8" || schedule.Name.ToUpper() == "EMIALING")
                {
                    action = async () =>
                    {
                        if (!emailingServices.EMAILServiceBusy())
                        {
                            await emailingServices.StartEmailingServiceAsync();
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

    #endregion



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
            #endregion
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

    #endregion

}
