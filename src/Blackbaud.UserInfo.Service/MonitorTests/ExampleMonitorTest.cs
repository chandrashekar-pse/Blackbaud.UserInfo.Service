using Blackbaud.ServiceStatus.Contracts;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.MonitorTests;

/// <summary>
/// Example of a monitor test.  Include a class like this for each test.
/// </summary>
public class ExampleMonitorTest : MonitorTest
{
    /// <summary>
    /// Example monitor test
    /// </summary>
    public ExampleMonitorTest() : base("Async Hello World Test") { }

    /// <summary>
    /// Execute the test
    /// </summary>
    /// <returns></returns>
    public override async Task<TestResult> Execute()
    {
        TestResult result = new();

        // do test things!
        await Task.Yield();
        result.Status = TestStatus.Succeeded;
        result.Details = "Some additional details.";

        return result;
    }
}