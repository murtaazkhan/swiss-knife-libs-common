using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace SwissKnife.Libs.Common.Extensions;

/// <summary>
/// Contains the Integration Startup
/// </summary>
public class ServiceStartup
{
    public IConfiguration Configuration { get; set; }

    /// <summary>
    /// This method configures services for startup
    /// </summary>
    /// <param name="services"></param>
    public virtual void ConfigureServices(IServiceCollection services)
    {
        try
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            //configures DI services
            services.AddOptions();
            services.AddHttpContextAccessor();

            services.AddControllers();
            services.AddEndpointsApiExplorer();

            //configure Swagger
            services.AddSwaggerGen();
        }
        catch (Exception arg)
        {
            Console.WriteLine($"Exception configuring services.  Shutting down.\nError: {arg}");
            throw;
        }
    }

    /// <summary>
    /// This method configures application with middlewares and extensions.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ApplicationException"></exception>
    public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        #region Validate Service Name
        Configuration = serviceProvider.GetRequiredService<IConfiguration>();
        string serviceName = Configuration?["SERVICE_NAME"];
        if (string.IsNullOrEmpty(serviceName))
        {
            Console.WriteLine("SERVICE_NAME environment variable must be set for this service.");
            throw new ApplicationException("SERVICE_NAME environment variable must be set for this service.");
        }
        #endregion

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseWebSockets();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGraphQL("/graphql");
        });
    }
}
