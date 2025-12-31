using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// Defines a contract for asynchronously retrieving secret values by name.
    /// </summary>
    /// <remarks>Implementations may retrieve secrets from various sources, such as environment variables,
    /// configuration files, or external secret stores. The retrieval process is asynchronous to support non-blocking
    /// operations, especially when accessing remote or secure storage.</remarks>
    public interface ISecretProvider
    {
        /// <summary>
        /// Asynchronously retrieves the value of a secret identified by its name.
        /// </summary>
        /// <param name="name">The name of the secret to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the value of the secret as a
        /// string, or null if the secret does not exist.</returns>
        Task<string> GetSecretAsync(string name);
    }
}
