using Blackbaud.UserInfo.Service.DataAccess.Models;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.DataAccess;

/// <summary>
/// An example data adapter
/// </summary>
[ExcludeFromCodeCoverage(Justification = "Data adapters typically do not get unit tested.")]
public class ExampleDataAdapter : IExampleDataAdapter
{
    /// <summary>
    /// Constructs a ExampleDataAdapter
    /// </summary>
    public ExampleDataAdapter()
    {
    }

    /// <summary>
    /// An async example data adapter operation.
    /// </summary>
    /// <returns></returns>
    public Task<Example> GetValues(CancellationToken cancellationToken)
    {
        // Task.FromResult isn’t required but simulating a true async operation
        // For instance, calling a database or another service
        cancellationToken.ThrowIfCancellationRequested();
        var response = new Example
        {
            FirstValue = "value1",
            SecondValue = "value2"
        };
        return Task.FromResult(response);
    }
}