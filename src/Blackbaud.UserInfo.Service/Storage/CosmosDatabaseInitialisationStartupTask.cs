using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// A background service that initializes the Cosmos DB database.
    /// </summary>
    /// <typeparam name="T">The type of the Cosmos DB service that implements <see cref="IUserCosmosDbService"/>.</typeparam>
    [ExcludeFromCodeCoverage]
    public class CosmosDatabaseInitialisationStartupTask<T> : IHostedService
        where T : IUserCosmosDbService
    {
        private readonly IUserCosmosDbService _imageCosmosDbInitializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CosmosDatabaseInitialisationStartupTask{T}"/> class.
        /// </summary>
        /// <param name="imageCosmosDbInitializer">The Cosmos DB initializer service.</param>
        public CosmosDatabaseInitialisationStartupTask(T imageCosmosDbInitializer)
        {
            _imageCosmosDbInitializer = imageCosmosDbInitializer;
        }

        /// <summary>
        /// Starts the background task to initialize the Cosmos DB database.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _imageCosmosDbInitializer.ValidateConnection(cancellationToken);
        }

        /// <summary>
        /// Stops the background task. This method is called when the application is shutting down.
        /// </summary>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A completed task.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
