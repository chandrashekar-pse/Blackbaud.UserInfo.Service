using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.UserInfo.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Services
{
    /// <summary>
    /// Service for interacting with Cosmos DB for image metadata.
    /// </summary>
    public class UserCosmosService : IUserCosmosService
    {
        private readonly IUserDataAdapter _cosmosDataAdapter;

        /// <summary>
        /// Constructor for UserCosmosService.
        /// </summary>
        /// <param name="cosmosDataAdapter">The data adapter for Cosmos DB operations.</param>
        public UserCosmosService(IUserDataAdapter cosmosDataAdapter)
        {
            _cosmosDataAdapter = cosmosDataAdapter;
        }

        /// <summary>
        /// Creates the document asynchronously.
        /// </summary>
        /// <param name="doc">The document to create.</param>
        /// <returns></returns>
        public async Task CreateDocumentAsync(User doc)
        {
            await _cosmosDataAdapter.CreateDocumentAsync(doc);
        }

        /// <summary>
        /// Fetches all documents by image name asynchronously.
        /// </summary>
        /// <param name="username">The image name to filter by.</param>
        /// <returns>A list of ImageReferenceDocument objects.</returns>
        public async Task<List<User>> GetAllByUserNameAsync(string username)
        {
            return await _cosmosDataAdapter.GetAllByUserNameAsync(username);
        }

        /// <summary>
        /// Gets the cosmos document asynchronously.
        /// </summary>
        /// <param name="username">The image name.</param>
        /// <param name="entityId">The entity ID.</param>
        /// <returns>The ImageReferenceDocument.</returns>
        public async Task<User> GetSingleDocumentAsync(string username, Guid entityId)
        {
            return await _cosmosDataAdapter.GetSingleDocumentAsync(username, entityId);
        }
    }
}