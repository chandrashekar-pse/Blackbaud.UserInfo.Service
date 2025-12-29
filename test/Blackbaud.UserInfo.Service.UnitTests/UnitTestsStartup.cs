using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.Testing.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blackbaud.UserInfo.Service.UnitTests;

public class UnitTestsStartup : Startup
{
    public UnitTestsStartup(IConfiguration configuration) : base(configuration) { }

    public void ConfigureUnitTestsServices(IServiceCollection services)
    {
        services.AddMock<IExampleDataAdapter>();
    }
}