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
}