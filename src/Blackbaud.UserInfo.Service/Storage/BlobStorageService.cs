using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// Provides methods for uploading, downloading, and deleting blobs in an Azure Blob Storage container.
    /// </summary>
    /// <remarks>This service encapsulates common blob operations and abstracts the underlying Azure Blob
    /// Storage SDK. All methods are asynchronous and thread-safe. Instances of this class are intended to be used with
    /// a specific blob container, as provided during construction.</remarks>
    public sealed class BlobStorageService : IBlobStorageService
    {
        private readonly BlobContainerClient _container;

        /// <summary>
        /// Initializes a new instance of the BlobStorageService class using the specified BlobContainerClient.
        /// </summary>
        /// <param name="container">The BlobContainerClient instance that provides access to the target Azure Blob container. Cannot be null.</param>
        public BlobStorageService(BlobContainerClient container) => _container = container;

        /// <summary>
        /// Uploads the specified content to the storage container as a blob with the given file name and content type.
        /// </summary>
        /// <param name="fileName">The name of the blob to create or overwrite in the storage container. Cannot be null or empty.</param>
        /// <param name="content">The stream containing the data to upload. The stream must be readable and positioned at the start of the
        /// content to upload.</param>
        /// <param name="contentType">The MIME type of the content to associate with the uploaded blob. If null, defaults to
        /// "application/octet-stream".</param>
        /// <returns>A string containing the name of the uploaded blob.</returns>
        public async Task<string> UploadAsync(string fileName, Stream content, string contentType)
        {
            var blob = _container.GetBlobClient(fileName);
            var headers = new BlobHttpHeaders { ContentType = contentType ?? "application/octet-stream" };
            await blob.UploadAsync(content, new BlobUploadOptions { HttpHeaders = headers });
            return blob.Name;
        }

        /// <summary>
        /// Asynchronously downloads the content of the specified file from the blob container.
        /// </summary>
        /// <remarks>The returned stream must be disposed by the caller when no longer needed. If the
        /// specified file does not exist in the container, the method returns null.</remarks>
        /// <param name="fileName">The name of the file to download from the blob container. Cannot be null or empty.</param>
        /// <returns>A stream containing the file's content if the file exists; otherwise, null.</returns>
        public async Task<Stream> DownloadAsync(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            if (!await blob.ExistsAsync()) return null;
            var resp = await blob.DownloadStreamingAsync();
            return resp.Value.Content;
        }

        /// <summary>
        /// Deletes the specified blob from the container if it exists.
        /// </summary>
        /// <remarks>If the specified blob does not exist, the method completes successfully and returns
        /// <see langword="false"/>. This operation is idempotent and does not throw an exception if the blob is
        /// missing.</remarks>
        /// <param name="fileName">The name of the blob to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the blob was
        /// deleted; otherwise, <see langword="false"/>.</returns>
        public async Task<bool> DeleteAsync(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            var resp = await blob.DeleteIfExistsAsync();
            return resp.Value;
        }
    }

}
