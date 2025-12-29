using Blackbaud.UserInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.DataAccess;

/// <summary>
/// An example data adapter interface
/// </summary>
public interface IUserDataAdapter
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
    /// <returns></returns>
    Task CreateDocumentAsync(User doc);

    /// <summary>
    /// Fetches all documents by user name asynchronously.
    /// </summary>
    /// <param name="userName">The user name to filter by.</param>
    /// <returns>A list of User objects.</returns>
    Task<List<User>> GetAllByUserNameAsync(string userName);

    /// <summary>
    /// Gets the cosmos document asynchronously.
    /// </summary>
    /// <param name="userName">The image name.</param>
    /// <param name="entityId">The entity ID.</param>
    /// <returns>The User.</returns>
    Task<User> GetSingleDocumentAsync(string userName, Guid entityId);
}