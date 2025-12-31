using Blackbaud.UserInfo.Service.DataAccess.Models;
using Blackbaud.UserInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.DataAccess;

/// <summary>
/// An example data adapter interface
/// </summary>
public interface IUserCosmosRepository
{
    /// <summary>
    /// An User business logic operation.
    /// </summary>
    /// <returns></returns>
    Task<User> GetValues(CancellationToken cancellationToken);

    /// <summary>
    /// Creates the cosmos document asynchronously.
    /// </summary>
    /// <param name="doc">The document to create.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task CreateDocumentAsync(User doc, CancellationToken cancellationToken = default);

    /// <summary>
    /// Fetches all documents by user name asynchronously.
    /// </summary>
    /// <param name="userName">The user name to filter by.</param>
    /// <returns>A list of User objects.</returns>
    Task<List<User>> GetAllByUserNameAsync(string userName);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="username"></param>
    /// <param name="entityId"></param>
    /// <returns></returns>
    Task<User> GetSingleDocumentAsync(string username, Guid entityId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entityId"></param>
    /// <returns></returns>
    Task<User> GetAsync(string id, Guid entityId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="entityId"></param>
    /// <returns></returns>
    Task<User> GetByEntityAsync(Guid entityId);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="u"></param>
    /// <returns></returns>
    Task<User> CreateAsync(User u);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entityId"></param>
    /// <param name="u"></param>
    /// <returns></returns>
    Task<User> UpsertAsync(string id, Guid entityId, User u);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="entityId"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(string id, Guid entityId);

}