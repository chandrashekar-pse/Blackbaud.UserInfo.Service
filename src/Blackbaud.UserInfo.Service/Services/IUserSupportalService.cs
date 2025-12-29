using Blackbaud.Gbs.Core.Models.Results;
using Blackbaud.UserInfo.Service.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Services
{
    /// <summary>
    /// Provides operations for managing image references and documents through the Supportal interface.
    /// Handles image reference deletion, complete image removal, and reference querying capabilities.
    /// </summary>
    public interface IUserSupportalService
    {
        ///// <summary>
        ///// Deletes a specific image reference based on the exact metadata criteria provided.
        ///// This operation removes a single link between an image and its associated entity based on the provided image name,
        ///// entity ID, and image type without affecting the image itself or other references.
        ///// </summary>
        ///// <param name="userData">The metadata criteria for identifying the specific image reference to delete,
        ///// requiring exact matches for image name, entity ID, and image type.</param>
        ///// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        ///// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation result.</returns>
        /////Task<Result> DeleteUserReferenceAsync(User userData, CancellationToken cancellationToken);

        ///// <summary>
        ///// Permanently deletes an image and all its associated reference documents.
        ///// This operation removes both the image from storage and all corresponding reference entries.
        ///// </summary>
        ///// <param name="userName">The unique identifier of the image to delete.</param>
        ///// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        ///// <returns>A <see cref="Task{Result}"/> representing the asynchronous operation result.</returns>
        /////Task<Result> DeleteUserAndDocumentsAsync(string userName, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves all reference documents associated with a specific image.
        /// Returns a collection of references that are linked to the specified image name.
        /// </summary>
        /// <param name="imageName">The unique identifier of the image to fetch references for.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Task{Result}"/> containing the collection of image references if found.</returns>
        Task<Result<User>> GetAllReferenceDocumentsAsync(string imageName, CancellationToken cancellationToken);
    }
}
