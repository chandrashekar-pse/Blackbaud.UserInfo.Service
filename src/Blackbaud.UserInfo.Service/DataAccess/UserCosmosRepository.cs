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
    /// A task that represents the asynchronous read operation. The task result contains the <see cref="User"/> if found; otherwise, <c>null</c>.
    /// </returns>
    public async Task<User> ReadAsync(string id, string pk)
    {
        try
        {
            var resp = await _container.ReadItemAsync<User>(id, new PartitionKey(pk));
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
    /// Asynchronously retrieves all users associated with the specified entity identifier.
    /// </summary>
    /// <returns>An asynchronous stream of <see cref="User"/> objects representing the users linked to the specified entity. The
    /// stream is empty if no users are found.</returns>

    // Repositories/UserRepository.cs
    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        var results = new List<User>();

        var query = new QueryDefinition("SELECT * FROM c");
        var options = new QueryRequestOptions
        {
            MaxItemCount = 100, // optional
            //EnableCrossPartitionQuery = true
        };

        using var iterator = _container.GetItemQueryIterator<User>(query, requestOptions: options);
        while (iterator.HasMoreResults)
        {
            var page = await iterator.ReadNextAsync();
            results.AddRange(page.Resource);
        }

        return results;
    }


    /// <summary>
    /// Asynchronously retrieves a user by identifier and phone number from the data store. 
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve. Cannot be null or empty.</param>
    /// <param name="entityId">The phone number associated with the user. Used as the partition key. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the user if found; otherwise, null.</returns>
    public async Task<User> GetAsync(string id, Guid entityId)
    {
        try
        {
            var resp = await _container.ReadItemAsync<User>(id, new PartitionKey(entityId.ToString()));
            return resp.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }


    /// <summary>
    /// Asynchronously retrieves all users associated with the specified phone number.
    /// </summary>
    /// <param name="entityId">The phone number to search for. Cannot be null or empty.</param>
    /// <returns>An asynchronous stream of <see cref="User"/> objects that have the specified phone number. The stream is empty
    /// if no users are found.</returns>

    public async Task<User> GetByEntityAsync(Guid entityId)
    {
        var q = new QueryDefinition("SELECT TOP 1 * FROM c WHERE c.entityId = @eid")
                    .WithParameter("@eid", entityId.ToString());

        using var it = _container.GetItemQueryIterator<User>(q, requestOptions: new QueryRequestOptions
        {
            PartitionKey = new PartitionKey(entityId.ToString())
        });

        while (it.HasMoreResults)
        {
            var page = await it.ReadNextAsync();
            return page.Resource.FirstOrDefault();
        }

        return null;
    }



    /// <summary>
    /// Asynchronously creates a new user in the data store.
    /// </summary>
    /// <param name="u">The user to create. Cannot be null. The user's Phone property is used as the partition key.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created user.</returns>
    public async Task<User> CreateAsync(User u)
    {
        var resp = await _container.CreateItemAsync(u, new PartitionKey(u.EntityId.ToString()));
        return resp.Resource;
    }


    /// <summary>
    /// Updates an existing user with the specified identifier and phone number, or creates a new user if one does not
    /// exist.
    /// </summary>
    /// <remarks>If the specified user does not exist, a new user is created. The method enforces that the id
    /// and Phone properties of the user object match the provided parameters. If an ETag is provided and does not match
    /// the current value, the update will fail.</remarks>
    /// <param name="id">The unique identifier of the user to update. Cannot be null or empty.</param>
    /// <param name="entityId">The phone number associated with the user. Used as the partition key. Cannot be null or empty.</param>
    /// <param name="u">The user object containing updated information. The object's id and Phone properties will be set to match the
    /// provided parameters.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the updated user if the operation
    /// succeeds; otherwise, null.</returns>

    public async Task<User> UpsertAsync(string id, Guid entityId, User u)
    {
        // Ensure route/query values are authoritative
        u.id = id;
        u.EntityId = entityId;

        var opts = new ItemRequestOptions();

        var resp = await _container.UpsertItemAsync(u, new PartitionKey(entityId.ToString()), opts);
        return resp.Resource;
    }


    /// <summary>
    /// Asynchronously deletes a user with the specified identifier and partition key from the data store.
    /// </summary>
    /// <param name="id">The unique identifier of the user to delete. Cannot be null or empty.</param>
    /// <param name="entityId">The partition key value associated with the user, typically the user's phone number. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the user was
    /// successfully deleted; otherwise, <see langword="false"/> if the user was not found.</returns>

    public async Task<bool> DeleteAsync(string id, Guid entityId)
    {
        try
        {
            await _container.DeleteItemAsync<User>(id, new PartitionKey(entityId.ToString()));
            return true;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}