namespace SwissKnife.Libs.Common.Scheduler.Models;

/// <summary>
/// Contains the Scheduler options
/// </summary>
public class SchedulerOptions
{
    /// <summary>
    /// Gets or sets Scheduler service name.
    /// </summary>
    public string SchedulerName { get; set; } = "Integration Scheduler";
    /// <summary>
    /// Gets or sets flag to enable scheduler execution.
    /// </summary>
    public bool EnablePolling { get; set; } = true;
    /// <summary>
    /// Gets or sets time interval in minutes to execute the scheduler methods.
    /// </summary>
    public int SchedulerTimeInMinutes { get; set; }
    /// <summary>
    /// Sets delegate method to execute when the Scheduler starts.
    /// </summary>
    public SchedulerDelegateMethod SchedulerStartMethod;
    /// <summary>
    /// Sets delegate method to execute when the Scheduler stops.
    /// </summary>
    public SchedulerDelegateMethod SchedulerEndMethod;
    
    /// <summary>
    /// Delegate method definition for scheduler methods.
    /// </summary>
    /// <returns></returns>
    public delegate Task SchedulerDelegateMethod();
}

