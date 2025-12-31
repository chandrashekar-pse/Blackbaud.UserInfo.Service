using Blackbaud.UserInfo.Service.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Controllers
{
    /// <summary>
    /// Provides API endpoints for uploading, downloading, and deleting files using blob storage.
    /// </summary>
    /// <remarks>This controller exposes RESTful endpoints for file management operations. All actions require
    /// multipart/form-data for uploads and use blob storage as the underlying persistence mechanism. The controller is
    /// intended for use in web applications that need to store and retrieve files via HTTP requests.</remarks>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Add this attribute to indicate authorization is required for all endpoints
    public sealed class FilesController : ControllerBase
    {
        private readonly IBlobStorageService _blobs;

        /// <summary>
        /// Initializes a new instance of the FilesController class using the specified blob storage service.
        /// </summary>
        /// <param name="blobs">The blob storage service to be used for file operations. Cannot be null.</param>
        public FilesController(IBlobStorageService blobs) => _blobs = blobs;

        // POST /api/files (multipart/form-data)
        /// <summary>
        /// Uploads a file to the server using a multipart/form-data POST request.
        /// </summary>
        /// <remarks>The maximum allowed request size is 50 MB. The uploaded file is stored with a unique
        /// name. To access the uploaded file, use the returned name in subsequent API requests.</remarks>
        /// <param name="file">The file to upload. Must not be null or empty.</param>
        /// <returns>A response containing the name of the uploaded file. Returns a 201 Created result with the file name if the
        /// upload is successful; otherwise, returns a 400 Bad Request if no file is provided.</returns>
        [HttpPost]
        [RequestSizeLimit(50_000_000)] // 50 MB example
        public async Task<ActionResult<object>> Upload([FromForm] IFormFile file)
        {
            if (file is null || file.Length == 0) return BadRequest("No file provided.");

            var fileName = $"{Guid.NewGuid()}-{file.FileName}";
            await using var stream = file.OpenReadStream();
            var name = await _blobs.UploadAsync(fileName, stream, file.ContentType);

            // Return the blob name; you can also return a SAS (see below)
            return Created($"/api/files/{name}", new { name });
        }

        /// <summary>
        /// Retrieves the specified file as a binary stream for download.
        /// </summary>
        /// <param name="name">The name of the file to download. This value is case-sensitive and must correspond to an existing file.</param>
        /// <returns>An <see cref="FileStreamResult"/> containing the file's binary data with the content type set to
        /// "application/octet-stream" if the file exists; otherwise, a <see cref="NotFoundResult"/> if the file is not
        /// found.</returns>
        // GET /api/files/{name}
        [HttpGet("{name}")]
        public async Task<IActionResult> Download([FromRoute] string name)
        {
            var stream = await _blobs.DownloadAsync(name);
            return stream is null ? NotFound() : File(stream, "application/octet-stream");
        }

        /// <summary>
        /// Deletes the file with the specified name from the storage container.
        /// </summary>
        /// <param name="name">The name of the file to delete. This value is case-sensitive and cannot be null or empty.</param>
        /// <returns>A <see cref="NoContentResult"/> if the file was successfully deleted; otherwise, a <see
        /// cref="NotFoundResult"/> if the file does not exist.</returns>
        // DELETE /api/files/{name}
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete([FromRoute] string name)
        {
            var ok = await _blobs.DeleteAsync(name);
            return ok ? NoContent() : NotFound();
        }
    }
}
