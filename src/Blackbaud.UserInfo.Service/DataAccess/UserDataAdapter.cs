using Blackbaud.UserInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.DataAccess;


/// <summary>
/// Data Adapter for User Cosmos operations
/// </summary>
/// <remarks>
/// constructor
/// </remarks>
/// <param name="userCosmosRepository"></param>
public class UserDataAdapter(IUserCosmosRepository userCosmosRepository) : IUserDataAdapter
{
    private readonly IUserCosmosRepository _userCosmosRepository = userCosmosRepository;

    /// <summary>
    /// An async user data adapter operation.
    /// </summary>
    /// <returns></returns>
    public Task<User> GetValues(CancellationToken cancellationToken)
    {
        // Task.FromResult isn’t required but simulating a true async operation
        // For instance, calling a database or another service
        cancellationToken.ThrowIfCancellationRequested();
        var response = new User
        {
            id = Guid.NewGuid().ToString(),
            EntityId = Guid.NewGuid(),
            FirstName = "David",
            LastName = "Warn",
            Phone = 1234567890
        };
        return Task.FromResult(response);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="doc"></param>
    /// <returns></returns>
    public async Task CreateDocumentAsync(User doc)
    {
        await _userCosmosRepository.CreateDocumentAsync(doc);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    public Task<List<User>> GetAllByUserNameAsync(string username)
    {
        return _userCosmosRepository.GetAllByUserNameAsync(username);
    }

    /// <summary>
    /// Gets the cosmos document asynchronously.
    /// </summary>
    /// <param name="username">The image name.</param>
    /// <param name="entityId">The entity ID.</param>
    /// <returns>The ImageReferenceDocument.</returns>
    public async Task<User> GetSingleDocumentAsync(string username, Guid entityId)
    {
        return await _userCosmosRepository.GetSingleDocumentAsync(username, entityId);
    }
}
