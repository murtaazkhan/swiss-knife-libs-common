using System.Diagnostics;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SwissKnife.Libs.Common.Extensions;
public static class ServiceBuilder
{
    /// <summary>
    /// Provides configurations to build a Web App
    /// </summary>
    /// <param name="builder"><see cref="WebApplicationBuilder"/></param>
    /// <returns></returns>
    public static WebApplication BuildWebApp(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureServices();
        var app = builder.Build();
        var env = app.Environment;
        app.ConfigureApp(builder, env);
        return app;
    }

    #region Private Methods
    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        try
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            services.AddOptions();
            services.AddHttpContextAccessor();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        catch (Exception value)
        {
            Console.WriteLine($"Exception configuring services.  Shutting down.\nError: {value}");
            throw;
        }

        return services;
    }

    private static void ConfigureApp(this IApplicationBuilder app, WebApplicationBuilder builder, IWebHostEnvironment env)
    {

        if (string.IsNullOrEmpty(builder.Configuration?["SERVICE_NAME"]))
        {
            Console.WriteLine("SERVICE_NAME environment variable must be set for this service.");
            throw new ApplicationException("SERVICE_NAME environment variable must be set for this service.");
        }

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllers();
            endpoints.MapGraphQL();
        });

    }
    #endregion
}

