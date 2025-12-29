using Blackbaud.UserInfo.Service.Authorization;
using Blackbaud.UserInfo.Service.Models;
using Blackbaud.Swagger.AspNetCore.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.Controllers;

/// <summary>
/// Example of a supportal controller, which will expose REST endpoints only accessible to blackbaud employees.
/// </summary>
[Route("v1/supportal")]
[SupportalEndpoint(null)]
public class ExampleSupportalController : ControllerBase
{
    private readonly BusinessLogic.ExampleService _exampleService;

    /// <summary>
    /// Constructs the SupportalController.
    /// Relies on a BusinessLogic.ExampleService class, provided by dependency injection.
    /// </summary>
    public ExampleSupportalController(BusinessLogic.ExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    /// <summary>
    /// Get values
    /// </summary>
    /// <remarks>
    /// An example endpoint that can be accessed only by employees
    /// </remarks>
    [HttpGet("values")]
    [SwaggerOperation(OperationId = "GetValues")]
    public Task<Example> GetValues(CancellationToken cancellationToken)
    {
        return _exampleService.GetAll(cancellationToken);
    }

    /// <summary>
    /// Get values detailed
    /// </summary>
    /// <remarks>
    /// An example endpoint that can be accessed only by team members
    /// </remarks>
    [HttpGet("values/detailed")]
    [SwaggerOperation(OperationId = "GetValuesDetailed")]
    [SupportalEndpoint(AuthorizationPolicies.EmpoweredUser)]
    public Task<Example> GetValuesDetailed(CancellationToken cancellationToken)
    {
        return _exampleService.GetAll(cancellationToken);
    }
}