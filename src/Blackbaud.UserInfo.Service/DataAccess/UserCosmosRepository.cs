using Blackbaud.Cosmos;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using User = Blackbaud.UserInfo.Service.Models.User;

namespace Blackbaud.UserInfo.Service.DataAccess;

/// <summary>
/// Repository for performing CRUD operations user items in Azure Cosmos DB.
/// </summary>
public sealed class UserCosmosRepository: IUserCosmosRepository
{
    private readonly ILogger _logger;
    private readonly ICosmosClientProvider _cosmosClientProvider;
    private readonly Container _container;
    private const string _containerId = "user-metadata";
    private const string _clientName = "nos-db";

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="cosmosClientProvider"></param>
    /// <param name="logger"></param>
    public UserCosmosRepository(ICosmosClientProvider cosmosClientProvider,
            ILogger<IUserCosmosRepository> logger)
    {
        _logger = logger;
        _cosmosClientProvider = cosmosClientProvider;
        var _config = cosmosClientProvider.GetClientConfiguration(_clientName);
        var cosmosClient = _cosmosClientProvider.GetClient(_clientName);
        _container = cosmosClient.GetContainer(_config.Databases?.First().DatabaseId, _containerId);

    }

    ///<inheritdoc/>
    public async Task CreateDocumentAsync(User document, CancellationToken cancellationToken = default)
    {

        while (true)
        {
            try
            {
                await _container.CreateItemAsync(
                    document,
                    new PartitionKeyBuilder().Add(document.FirstName.ToString()).Add(document.EntityId.ToString()).Build(),
                    cancellationToken: cancellationToken
                );
                return;
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            //catch (CosmosException ex)
            //{
            //    throw;
            //}
            //catch (Exception ex)
            //{
            //    // Log and rethrow general exceptions
            //    _logger.LogError(ex, $"Unexpected error while creating document: {ex.Message} ImageName: {document.ImageName}");
            //    throw new MetadataUpdateFailedException();
            //}
        }
    }

    /// <summary>
    /// Asynchronously reads a <see cref="User"/> item by its ID and partition key.
    /// </summary>
    /// <param name="id">The ID of the notification to read.</param>
    /// <param name="pk">The partition key of the notification.</param>
    /// <returns>
    /// A task that represents the asynchronous read operation. The task result contains the <see cref="Service.Models.User"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<User> ReadAsync(string id, string pk)
    {
        try
        {
            var resp = await _container.ReadItemAsync<Service.Models.User>(id, new PartitionKey(pk));
            return resp.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    /// <summary>
    /// Get values
    /// </summary>
    /// <remarks>
    /// An example endpoint that can be accessed only by employees
    /// </remarks>
    public async Task<User> GetValues(CancellationToken cancellationToken)
    {
        var query = new QueryDefinition("SELECT * FROM c OFFSET 0 LIMIT 1");

        using var iterator = _container.GetItemQueryIterator<User>(query);

        if (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.Resource.FirstOrDefault();
        }

        return null;
    }

    /// <summary>
    /// Get values detailed
    /// </summary>
    /// <remarks>
    /// An example endpoint that can be accessed only by team members
    /// </remarks>
   
    public async Task<List<User>> GetAllByUserNameAsync(string username)
    {
        var users = new List<User>();

        var query = new QueryDefinition(
          "SELECT * FROM c WHERE c.UserName = @username")
          .WithParameter("@username", username);

        using var iterator = _container.GetItemQueryIterator<User>(query);

        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            users.AddRange(response);
        }

        return users;
    }

    /// <summary>
    /// Gets a single user document based on username and entityId
    /// </summary>
    public async Task<User> GetSingleDocumentAsync(string username, Guid entityId)
    {
        var query = new QueryDefinition(
          "SELECT * FROM c WHERE c.UserName = @username AND c.EntityId = @entityentityId")
          .WithParameter("@username", username)
          .WithParameter("@entityId", entityId);


        using var iterator = _container.GetItemQueryIterator<User>(query);


        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();
            return response.Resource.FirstOrDefault();
        }


        return null;
    }

    /// <summary>
    /// Asynchronously lists all <see cref="Service.Models.User"/> items for the specified partition key.
    /// </summary>
    /// <param name="pk">The partition key to filter notifications.</param>
    /// <returns>An async enumerable of <see cref="Service.Models.User"/> objects.</returns>
    public async IAsyncEnumerable<User> ListAsync(string pk)
    {
        var q = new QueryDefinition("SELECT * FROM c WHERE c.PartitionKey = @pk")
                    .WithParameter("@pk", pk);
        using var it = _container.GetItemQueryIterator<Service.Models.User>(q);
        while (it.HasMoreResults)
        {
            foreach (var item in await it.ReadNextAsync())
                yield return item;
        }
    }
}