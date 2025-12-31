using Blackbaud.UserInfo.Service.Constants;
using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.UserInfo.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Controllers
{
    /// <summary>
    /// Provides API endpoints for managing user resources.
    /// </summary>
    /// <remarks>This controller is configured with the route "api/[controller]" and is intended to be used as
    /// part of an ASP.NET Core Web API. It serves as the entry point for HTTP requests related to user operations, such
    /// as retrieving, creating, updating, or deleting users. The controller inherits from <see cref="ControllerBase"/>,
    /// enabling standard API controller features such as model binding, validation, and response formatting.</remarks>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthSchemeNames.Sas)]
    public class UsersController : ControllerBase
    {
        private readonly IUserCosmosRepository _repo;
        /// <summary>
        /// Initializes a new instance of the UsersController class with the specified user repository.
        /// </summary>
        /// <param name="repo">The user repository used to access and manage user data in the controller. Cannot be null.</param>
        public UsersController(IUserCosmosRepository repo) => _repo = repo;

        /// <summary>
        /// Retrieves a user by identifier within the specified entity partition.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve. Supplied as a route parameter.</param>
        /// <param name="entityId">The partition key representing the entity context. Must not be <see cref="Guid.Empty"/>. Supplied as a query
        /// parameter.</param>
        /// <returns>An <see cref="ActionResult{User}"/> containing the user if found; otherwise, a <see cref="NotFoundResult"/>
        /// if the user does not exist, or a <see cref="BadRequestObjectResult"/> if <paramref name="entityId"/> is
        /// invalid.</returns>
        // GET /api/users/{id}?entityId=...
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetById([FromRoute] string id, [FromQuery] Guid entityId)
        {
            if (entityId == Guid.Empty)
                return BadRequest("Query 'entityId' (partition key) is required.");

            var user = await _repo.GetAsync(id, entityId);
            return user is null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Retrieves the user associated with the specified entity identifier.
        /// </summary>
        /// <param name="entityId">The unique identifier of the entity for which to retrieve the user.</param>
        /// <returns>An <see cref="ActionResult{User}"/> containing the user associated with the specified entity identifier if
        /// found; otherwise, a 404 Not Found response.</returns>
        // GET /api/users/by-entity/{entityId}
        [HttpGet("by-entity/{entityId:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> GetByEntityId([FromRoute] Guid entityId)
        {
            var user = await _repo.GetByEntityAsync(entityId);
            return user is null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Creates a new user resource with the specified details. 
        /// </summary>
        /// <remarks>If the <paramref name="u"/> parameter includes an explicit user ID, it will be used;
        /// otherwise, a default GUID is assigned. The response includes a location header referencing the newly created
        /// user resource.</remarks>
        /// <param name="u">The user information to create. The <see cref="User.EntityId"/> property must be set to a non-empty value,
        /// as it serves as the partition key.</param>
        /// <returns>An <see cref="ActionResult{User}"/> containing the created user resource. Returns a 201 Created response
        /// with the user data if successful; otherwise, returns a 400 Bad Request if the required EntityId is missing.</returns>
        // POST /api/users
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Create([FromBody] User u)
        {
            if (u.EntityId == Guid.Empty)
                return BadRequest("EntityId is required and is the partition key.");
            // id is 'required' with default; if caller sent an id we respect it; otherwise the default GUID applies.

            var created = await _repo.CreateAsync(u);
            return CreatedAtAction(nameof(GetById), new { id = created.id, entityId = created.EntityId }, created);
        }

        /// <summary>
        /// Creates a new user or updates an existing user with the specified identifier and entity association.
        /// </summary>
        /// <remarks>If the specified user does not exist, a new user is created. If the user exists, the
        /// existing record is updated. The operation supports optimistic concurrency via the optional ETag
        /// header.</remarks>
        /// <param name="id">The unique identifier of the user to create or update. Supplied from the route parameter.</param>
        /// <param name="entityId">The identifier of the associated entity. Must not be <see cref="Guid.Empty"/>. Supplied as a query
        /// parameter.</param>
        /// <param name="u">The user data to be created or updated. Supplied in the request body.</param>
        /// <returns>An <see cref="ActionResult{User}"/> containing the created or updated user if successful; otherwise, a bad
        /// request result if the input is invalid.</returns>
        // PUT /api/users/{id}?entityId=...
        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Upsert(
            [FromRoute] string id,
            [FromQuery] Guid entityId,
            [FromBody] User u)
        {
            if (entityId == Guid.Empty)
                return BadRequest("Query 'entityId' is required.");

            var updated = await _repo.UpsertAsync(id, entityId, u);
            return Ok(updated);
        }

        /// <summary>
        /// Deletes the user with the specified identifier from the entity identified by the given entity ID.
        /// </summary>
        /// <param name="id">The unique identifier of the user to delete. This value is provided in the route.</param>
        /// <param name="entityId">The unique identifier of the entity from which the user will be deleted. This value is provided as a query
        /// parameter and must not be <see cref="Guid.Empty"/>.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation. Returns <see cref="NoContentResult"/>
        /// if the user was deleted successfully; <see cref="NotFoundResult"/> if the user was not found; or <see
        /// cref="BadRequestObjectResult"/> if <paramref name="entityId"/> is not specified.</returns>
        // DELETE /api/users/{id}?entityId=...
        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete([FromRoute] string id, [FromQuery] Guid entityId)
        {
            if (entityId == Guid.Empty)
                return BadRequest("Query 'entityId' is required.");

            var ok = await _repo.DeleteAsync(id, entityId);
            return ok ? NoContent() : NotFound();
        }

    }
}
