using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SwissKnife.Libs.Common.Scheduler.Models;

namespace SwissKnife.Libs.Common.Scheduler.Extensions;

/// <summary>
/// This method adds a hosted service
/// </summary>
/// <param name="services">The service collection instance.</param>
/// <param name="options">The <see cref="SchedulerOptions"/> to use for hosted service.</param>
public static class SchedulerExtensions
{
    public static void AddSchedulerService(this IServiceCollection services, SchedulerOptions options)
    {
        services.AddSingleton<IHostedService>(serviceProvider => 
                                                new SchedulerService(
                                                    serviceProvider.GetService<ILogger<SchedulerService>>(),
                                                    options));

    }
}
