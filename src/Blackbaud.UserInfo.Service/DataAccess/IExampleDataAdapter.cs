using Blackbaud.UserInfo.Service.DataAccess.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.DataAccess;

/// <summary>
/// An example data adapter interface
/// </summary>
public interface IExampleDataAdapter
{
    /// <summary>
    /// An example business logic operation.
    /// </summary>
    /// <returns></returns>
    Task<Example> GetValues(CancellationToken cancellationToken);
}