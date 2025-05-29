namespace CampusLearn.Services.Domain;

public class PersonalHostedService : IHostedLifecycleService
{
    #region -- protected properties --

    protected readonly ILogger<PersonalHostedService> logger;
    private readonly ScheduleManager scheduleManager;

    #endregion -- protected properties --

    public PersonalHostedService(ILogger<PersonalHostedService> logger, ScheduleManager scheduleManager)
    {
        this.logger = logger;
        this.scheduleManager = scheduleManager;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Start {this}");
        return scheduleManager.ConfigureSchedulesAsync(true);
    }

    public Task StartedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Started {this}");
        return Task.CompletedTask;
    }

    public Task StartingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Starting {this}");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Stop {this}");
        return Task.CompletedTask;
    }

    public Task StoppedAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Stopped {this}");
        return Task.CompletedTask;
    }

    public Task StoppingAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation($"Stopping {this}");
        return Task.CompletedTask;
    }
}
