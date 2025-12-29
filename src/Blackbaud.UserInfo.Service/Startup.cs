using Blackbaud.Core.WebService.AspNetCore;
using Blackbaud.Core.WebService.Contracts;
using Blackbaud.UserInfo.Service.Authorization;
using Blackbaud.UserInfo.Service.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blackbaud.UserInfo.Service;

/// <summary>
/// Startup class for configuring the web service.
/// </summary>
public class Startup
{
    private IConfiguration Configuration { get; }

    /// <summary>
    /// Constructor to inject Configuration
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration) => Configuration = configuration;

    /// <summary>
    /// Configure dependency injection container
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure an example Business Logic class
        services.AddSingleton<DataAccess.IExampleDataAdapter>(
            _ => new DataAccess.ExampleDataAdapter()
        );
        // Configure an example Business Logic class
        services.AddScoped((serviceProvider) =>
            {
                var userPermissions = serviceProvider.GetService<Permissions.Resolver.UserPermissions>();
                var dataAdapter = serviceProvider.GetService<DataAccess.IExampleDataAdapter>();

                return new BusinessLogic.ExampleService(
                    userPermissions,
                    dataAdapter
                );
            })
            .AddScoped(provider => new Permissions.Resolver.UserPermissions(
                provider.GetService<IRequestContext>()?.UserAccess ?? new UserAccess()
            ));

        services.AddMonitorTest<MonitorTests.ExampleMonitorTest>();
        services.AddServices();
        services.AddAuthorizationPolicies();
    }

    /// <summary>
    /// Configure ASP.NET Core MVC application
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app) => app.ConfigureBlackbaud<Startup>();
}