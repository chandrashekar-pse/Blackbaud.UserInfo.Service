using Blackbaud.Blob.Storage;
using Microsoft.Extensions.Configuration;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// Provides configuration settings for the image storage account, including container names,
    /// retry policies, and storage account details. Inherits from <see cref="BlackbaudStorageAccountConfig"/>
    /// and implements <see cref="IStorageAccountConfig"/>.
    /// </summary>
    public class UserBlobStorageAccountConfig : BlackbaudStorageAccountConfig, IStorageAccountConfig
    {
        /// <summary>
        /// The configuration instance used to retrieve settings.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserBlobStorageAccountConfig"/> class.
        /// </summary>
        /// <param name="configuration">The configuration provider for retrieving settings.</param>
        public UserBlobStorageAccountConfig(IConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// The name of the blob container used for storing images.
        /// </summary>
        public readonly string ContainerName = "nos-db";

        /// <summary>
        /// Gets the storage account name used for image management.
        /// </summary>
        protected override string StorageName => "user-storage-account";

        ///// <summary>
        ///// Gets the initial delay for retry attempts, in milliseconds.
        ///// </summary>
        //public new TimeSpan RetryDelay => TimeSpan.FromMilliseconds(_configuration.GetValue<int>("RetryDelay", 100));

        ///// <summary>
        ///// Gets the maximum number of retry attempts for storage operations.
        ///// </summary>
        //public new int RetryMaxRetries => _configuration.GetValue<int>("RetryMaxRetries", 3);

        ///// <summary>
        ///// Gets the maximum delay cap for exponential backoff, in seconds.
        ///// </summary>
        //public TimeSpan MaxRetryDelay => TimeSpan.FromSeconds(_configuration.GetValue<int>("MaxRetryDelaySeconds", 30));
    }
}