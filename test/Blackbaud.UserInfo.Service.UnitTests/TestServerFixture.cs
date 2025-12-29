using Blackbaud.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace Blackbaud.UserInfo.Service.UnitTests;

public class TestServerFixture : TestServerFixture<UnitTestsStartup>
{

}

public class TestServerFixture<TStartup> : TestServerFixtureBase where TStartup : Startup
{
    public override IHostBuilder BuildTestFixtureWebHost()
    {
        var builder = Program.BuildWebHost(Array.Empty<string>())
            .ConfigureWebHost(webHostBuilder => webHostBuilder
                .UseContentRoot(Path.GetFullPath(Path.Combine(
                    AppContext.BaseDirectory,
                    "..",
                    "..",
                    "..",
                    "..",
                    "src",
                    "Blackbaud.UserInfo.Service")))
                .UseEnvironment("UnitTests")
                .UseStartup<TStartup>()
                .UseTestServer()
            );

        return builder;
    }
}