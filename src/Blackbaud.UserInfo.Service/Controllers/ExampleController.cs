using Blackbaud.Core.WebService.AspNetCore.Filters;
using Blackbaud.Core.WebService.Contracts;
using Blackbaud.UserInfo.Service.Authorization;
using Blackbaud.UserInfo.Service.Models;
using Blackbaud.Swagger.AspNetCore;
using Blackbaud.Swagger.AspNetCore.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Controllers;

/// <summary>
/// Example of an API controller, which will expose REST endpoints.
/// </summary>
[Route("v1/example")]
[ApiController]
[SKYAPIEndpoint(EndpointServiceLevel.Preview)]
public class ExampleController : ControllerBase
{
    private readonly BusinessLogic.ExampleService _exampleService;
    private readonly IRequestContext _requestContext;
    internal const string SAS_SCOPE_ADVANCED_ACCESS = "read-3";

    /// <summary>
    /// Constructs the ExampleController.
    /// Relies on a BusinessLogic.ExampleService and IRequestContext classes, provided by dependency injection.
    /// </summary>
    public ExampleController(BusinessLogic.ExampleService exampleService, IRequestContext requestContext)
    {
        _exampleService = exampleService;
        _requestContext = requestContext;
    }

    /// <summary>
    /// Get values
    /// </summary>
    /// <remarks>
    /// An example endpoint that can be accessed anonymously with a GET to /v1/example/values
    /// </remarks>
    [HttpGet("values")]
    [AllowAnonymous]
    [SwaggerOperation(OperationId = "GetValues")]
    public Task<Example> GetValues(CancellationToken cancellationToken)
    {
        return _exampleService.GetAll(cancellationToken);
    }

    /// <summary>
    /// An example endpoint that can be accessed with a GET to /v1/example/values2
    /// Explicitly specifying an Authorize attribute with an AuthenticationSchemes of "BBID", "SAS", or "BBID,SAS" lets you control which security methods are allowed for the endpoint.
    /// (The authentication options supported by the service are enabled via applicationsettings.json.)
    /// Additionally, a policy is specified in order to further authorize the endpoint - this is typically limited by permissions (1bb.perms) in the JWT.
    /// Policies are evaluated regardless of the authentication scheme used, so they should account for BBID or SAS requests if the endpoint's AuthenticationSchemes allow both.
    ///
    /// AuthenticationUserIdContextFilter and EnvironmentIdContextFilter add checks that ensure that an AuthenticationUserId and EnvironmentId are provided on the request.
    /// </summary>
    [HttpGet("values2")]
    [Authorize(AuthenticationSchemes = "BBID", Policy = AuthorizationPolicies.UsersViewOnly)]
    [AuthenticationUserIdContextFilter(true)]
    [EnvironmentIdContextFilter(true)]
    [ExcludeEndpointFromSKYAPI("This endpoint is used for a very specific reason on the front end so doesn't make sense to make available publicly.")]
    public Example GetFromBrowserOrTrustedService()
    {
        // Example for how to retrieve the environment ID or user ID, regardless of whether it is a SAS or BBID call.
        // The environment ID can come from the 'Blackbaud-EnvironmentId' header or 'envid' query string.
        // The user ID can come from the 'Blackbaud-Authentication-UserId' header or the 'Blackbaud-Authentication-UserId' query string for SAS, or the BBID JWT.

        var currentUserId = _requestContext.AuthenticationUserId;
        var environmentId = _requestContext.EnvironmentId;

        return _exampleService.GetContextData(currentUserId, environmentId);
    }

    /// <summary>
    /// Get values 3
    /// </summary>
    /// <remarks>
    /// An example endpoint that can be accessed via BBID or SAS (with specific scope) with a GET to /v1/example/values3
    /// </remarks>
    [HttpGet("values3")]
    [Authorize(AuthenticationSchemes = "BBID,SAS")]
    [ServiceAuthorizationScopeFilter(SAS_SCOPE_ADVANCED_ACCESS)]
    [SwaggerOperation(OperationId = "GetValues3")]
    public Task<Example> GetSas(CancellationToken cancellationToken)
    {
        return _exampleService.GetAllWithPermissionsCheckAsync(cancellationToken);
    }
}