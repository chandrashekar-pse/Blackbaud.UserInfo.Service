using Blackbaud.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// Startup class for configuring the cosmos db for image management
    /// </summary>
    public interface IUserCosmosDbService
    {
        /// <summary>
        /// Validates the connection to the cosmos db
        /// </summary>
        /// <returns></returns>
        Task ValidateConnection(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Startup class for configuring the cosmos db for image management
    /// </summary>
    /// <remarks>
    /// constructor
    /// </remarks>
    /// <param name="cosmosClientProvider"></param>
    /// <param name="logger"></param>
    [ExcludeFromCodeCoverage]
    public class UserCosmosDbService(
        ICosmosClientProvider cosmosClientProvider,
        ILogger<UserCosmosDbService> logger
        //,IOptions<GlobalConfig> config
        ) : IUserCosmosDbService
    {
        //private readonly GlobalConfig _config = config.Value;
        private readonly ILogger<UserCosmosDbService> _logger = logger;
        private readonly ICosmosClientProvider _clientProvider = cosmosClientProvider;

        /// <inheritdoc/>
        [ExcludeFromCodeCoverage]
        public async Task ValidateConnection(CancellationToken cancellationToken)
        {
            var cosmosConfig = _clientProvider.GetClientConfiguration("nos-db");

            if (cosmosConfig == null)
            {
                _logger.LogError("Cosmos configuration for user-management is not found.");
                throw new InvalidOperationException("Cosmos configuration for user-management is not found.");
            }
            // Use a valid containerId from configuration for validation
            if (cosmosConfig.Databases == null || !cosmosConfig.Databases.Any())
            {
                _logger.LogError("No databases configured in Cosmos settings.");
                throw new InvalidOperationException("No databases configured in Cosmos settings.");
            }
            var dbConfig = cosmosConfig.Databases.First();
            if (dbConfig?.Containers == null || !dbConfig.Containers.Any())
            {
                _logger.LogError("No containers configured in Cosmos database settings.");
                throw new InvalidOperationException("No containers configured in Cosmos database settings.");
            }
            var containerId = dbConfig.Containers.First().ContainerId;
            var client = _clientProvider.GetClient("nos-db");

            // Specify null for throughput and explicitly cast to int? to resolve ambiguity

            var dbResponse = await client.CreateDatabaseIfNotExistsAsync(dbConfig.DatabaseId, (ThroughputProperties)null, (RequestOptions)null, cancellationToken);
            var db = dbResponse.Database;
            var containerConfig = dbConfig.Containers.First(c => c.ContainerId == containerId);
            var containerProperties = new ContainerProperties
            {
                Id = containerConfig.ContainerId,
                PartitionKeyDefinitionVersion = PartitionKeyDefinitionVersion.V2,
                PartitionKeyPath = "/EntityId",
                //DefaultTimeToLive = _config.RetentionPeriodDays * 86400 // Convert days to seconds
            };

            // Create the container if it does not exist
            await db.CreateContainerIfNotExistsAsync(containerProperties, (ThroughputProperties)null, null, cancellationToken);
        }
    }
}