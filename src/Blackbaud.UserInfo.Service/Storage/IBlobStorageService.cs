using System.IO;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Asynchronously uploads the specified file content to the remote storage and returns a unique identifier for
        /// the uploaded file.
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded. Cannot be null or empty.</param>
        /// <param name="content">A stream containing the file data to upload. The stream must be readable and positioned at the start of the
        /// content to upload. Cannot be null.</param>
        /// <param name="contentType">The MIME type of the file content (for example, "image/png" or "application/pdf"). Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous upload operation. The task result contains a string that uniquely
        /// identifies the uploaded file.</returns>
        Task<string> UploadAsync(string fileName, Stream content, string contentType);
       
        /// <summary>
        /// Asynchronously downloads the specified file and returns a stream containing its contents.
        /// </summary>
        /// <param name="fileName">The name of the file to download. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a stream with the file's
        /// contents.</returns>
        Task<Stream> DownloadAsync(string fileName);

        /// <summary>
        /// Asynchronously deletes the specified file from the storage system.  
        /// </summary>
        /// <param name="fileName">The name of the file to delete. Cannot be null or empty.</param>
        /// <returns>A task that represents the asynchronous delete operation. The task result is <see langword="true"/> if the
        /// file was successfully deleted; otherwise, <see langword="false"/>.</returns>
        Task<bool> DeleteAsync(string fileName);

    }
}
