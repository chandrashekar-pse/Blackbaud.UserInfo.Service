using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blackbaud.UserInfo.Service.UnitTests;

public class DependencyInjectionTestStartup : Startup
{
    public DependencyInjectionTestStartup(IConfiguration configuration) : base(configuration)
    {

    }

    public virtual void ConfigureUnitTestsServices(IServiceCollection services)
    {
        //Add the service collection to the container so we can access it in a unit test
        services.AddSingleton(services);
    }
}