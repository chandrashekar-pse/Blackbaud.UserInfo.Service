using Blackbaud.Testing.Tests;
using Microsoft.AspNetCore.TestHost;
using System.Reflection;

namespace Blackbaud.UserInfo.Service.UnitTests;

public class ServiceTests : CoreSkyApiServiceTests
{
    private static TestServerFixture _fixture;
    private static TestServerFixture<DependencyInjectionTestStartup> _dependencyInjectionFixture;

    public override Assembly AssemblyUnderTest => typeof(Startup).Assembly;

    public override TestServer TestServer
    {
        get
        {
            if (_fixture == null)
            {
                _fixture = new();
            }
            return _fixture.TestServer;
        }
    }

    public override TestServer DependencyInjectionTestServer
    {
        get
        {
            if (_dependencyInjectionFixture == null)
            {
                _dependencyInjectionFixture = new();
            }
            return _dependencyInjectionFixture.TestServer;
        }
    }
}