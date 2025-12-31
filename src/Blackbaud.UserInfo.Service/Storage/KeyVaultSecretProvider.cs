using Azure.Security.KeyVault.Secrets;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// Provides secret retrieval functionality from an Azure Key Vault using a specified SecretClient.
    /// </summary>
    /// <remarks>This class is a sealed implementation of the ISecretProvider interface that enables
    /// asynchronous access to secrets stored in Azure Key Vault. It is intended for use in scenarios where secure,
    /// centralized secret management is required. Thread safety is determined by the underlying SecretClient
    /// instance.</remarks>
    public sealed class KeyVaultSecretProvider : ISecretProvider
    {
        private readonly SecretClient _client;

        /// <summary>
        /// Initializes a new instance of the KeyVaultSecretProvider class using the specified SecretClient.
        /// </summary>
        /// <param name="client">The SecretClient instance used to access Azure Key Vault secrets. Cannot be null.</param>
        public KeyVaultSecretProvider(SecretClient client) => _client = client;

        /// <summary>
        /// Asynchronously retrieves the value of a secret with the specified name.
        /// </summary>
        /// <param name="name">The name of the secret to retrieve. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the secret value as a string, or
        /// null if the secret is not found or an error occurs.</returns>
        public async Task<string> GetSecretAsync(string name)
        {
            try
            {
                var s = await _client.GetSecretAsync(name);
                return s.Value.Value;
            }
            catch { return null; }
        }
    }
}
