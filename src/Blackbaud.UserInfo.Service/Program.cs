using Blackbaud.Core.Hosting;
using Blackbaud.Core.WebService.AspNetCore;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service;

/// <summary>
/// Entry point class for the web service.
/// </summary>
public static class Program
{
    /// <summary>
    /// Entry point function for the web service.
    /// </summary>
    public static async Task Main(string[] args) => await BuildWebHost(args).Build().RunAsync();

    /// <summary>
    /// Creates a IHostBuilder for the application.
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder BuildWebHost(string[] args) => new HostBuilder().ConfigureBlackbaudHost(args).ConfigureBlackbaudWebHost<Startup>();
}