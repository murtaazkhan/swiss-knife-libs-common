using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwissKnife.Libs.Common.Scheduler.Models;

namespace SwissKnife.Libs.Common.Scheduler;

/// <summary>
/// Contains the Scheduler Service implementation
/// </summary>
public class SchedulerService : IHostedService
{
    private readonly ILogger<SchedulerService> _logger;
    private readonly SchedulerOptions _options;
    /// <summary>
    /// Constructor to initialize injected services
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="options"></param>
    public SchedulerService(ILogger<SchedulerService> logger, SchedulerOptions options) 
    {
        _logger = logger;
        _options = options;
    }

    /// <summary>
    /// This method is executed when the hosted service starts.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task StartAsync(CancellationToken cancellationToken)
    {
        if (_options.EnablePolling)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        _logger.LogInformation($"{_options.SchedulerName} polling started...");
                        await _options.SchedulerStartMethod(); // executes the delegate method
                        _logger.LogInformation($"{_options.SchedulerName} polling ended.");
                        Thread.Sleep(TimeSpan.FromMinutes(_options.SchedulerTimeInMinutes)); // sleeps thread until next execution
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"Error starting polling for {_options.SchedulerName} : {ex.Message}");
                    }
                }
            }, cancellationToken);
        }
        
        return Task.CompletedTask;
    }

    /// <summary>
    /// This method is executed when the hosted service stops.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public virtual Task StopAsync(CancellationToken cancellationToken)
    {
        if(_options.EnablePolling)
        {
            Task.Run(async () =>
            {
                try
                {
                    _logger.LogInformation($"{_options.SchedulerName} polling stopped.");
                    await _options.SchedulerEndMethod();// executes the delegate method
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error stopping poll for {_options.SchedulerName} : {ex.Message}");
                }
                
            }, cancellationToken);
        }
        
        return Task.CompletedTask;
    }
}

