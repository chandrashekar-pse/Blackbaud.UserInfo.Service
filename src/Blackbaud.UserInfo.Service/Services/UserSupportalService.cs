using Blackbaud.Gbs.Core.Models.Results;
using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.UserInfo.Service.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Services
{
    /// <summary>
    /// Provides operations for managing image references and documents through the Supportal Service.
    /// Handles image reference deletion, complete image removal, and reference querying capabilities.
    /// </summary>
    /// <remarks>
    /// Constructor for ImageSupportalService.
    /// </remarks>
    /// <param name="cosmosDataAdapter">The data adapter for Cosmos DB operations.</param>
    /// <param name="logger"></param>
    public class UserSupportalService(IUserDataAdapter cosmosDataAdapter, ILogger<UserSupportalService> logger) : IUserSupportalService
    {
        private readonly IUserDataAdapter _cosmosDataAdapter = cosmosDataAdapter;
        private readonly ILogger<UserSupportalService> _logger = logger;

        ///// <inheritdoc />
        //public async Task<Result> DeleteUserReferenceAsync(User userdata, CancellationToken cancellationToken)
        //{
        //    cancellationToken.ThrowIfCancellationRequested();

        //    var imageReferenceDocument = await _cosmosDataAdapter.GetSingleDocumentAsync(
        //        userdata.FirstName,
        //        userdata.EntityId,
        //        userdata.LastName);

        //    if (imageReferenceDocument == null)
        //    {
        //        _logger.LogDebug(
        //            "Image reference document not found for ImageName: {ImageName}, EntityId: {EntityId}, ImageType: {ImageType}",
        //            imageReferenceMetadata.ImageName,
        //            imageReferenceMetadata.EntityId,
        //            imageReferenceMetadata.ImageType);

        //        return Result.Failure(DomainCodes.ImageWriteService.DeleteReferenceFailed);
        //    }

        //    await _cosmosDataAdapter.DeleteDocumentByIdAsync(imageReferenceDocument);

        //    _logger.LogInformation(
        //        "Deleted image reference for ImageName: {ImageName}, EntityId: {EntityId}, ImageType: {ImageType}",
        //        imageReferenceMetadata.ImageName,
        //        imageReferenceMetadata.EntityId,
        //        imageReferenceMetadata.ImageType);

        //    return Result.Success();
        //}

        ///// <inheritdoc />
        //public async Task<Result> DeleteImageAndDocumentsAsync(string imageName, CancellationToken cancellationToken)
        //{
        //    return await Task.FromResult(Result.Success());
        //}

        /// <inheritdoc />
        public async Task<Result<User>> GetAllReferenceDocumentsAsync(string imageName, CancellationToken cancellationToken)
        {
            return await Task.FromResult(
                Result<User>.Success(
                    new User()
                    {
                        id = Guid.NewGuid().ToString(),
                        EntityId = Guid.NewGuid(),
                        FirstName = "David",
                        LastName = "Warn",
                        Phone = 1234567890
                    }
                )
            );
        }
    }
}