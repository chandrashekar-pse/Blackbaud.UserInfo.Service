using Blackbaud.UserInfo.Service.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Blackbaud.UserInfo.Service.BusinessLogic;
using AllocationManagement = Permissions.Resolver.PermissionFlags.Generalledger.FENXT.Allocationmanagement;

/// <summary>
/// An example business logic domain
/// </summary>
public class ExampleService
{
    private readonly DataAccess.IExampleDataAdapter _exampleDataAdapter;
    private readonly Permissions.Resolver.UserPermissions _userPermissions;

    /// <summary>
    /// Constructs an Example
    /// </summary>
    public ExampleService(Permissions.Resolver.UserPermissions userPermissions,
        DataAccess.IExampleDataAdapter exampleDataAdapter)
    {
        _exampleDataAdapter = exampleDataAdapter ?? throw new ArgumentNullException(nameof(exampleDataAdapter));
        _userPermissions = userPermissions ?? throw new ArgumentNullException(nameof(userPermissions));
    }

    /// <summary>
    /// An async example business logic operation.
    /// </summary>
    /// <returns></returns>
    public async Task<Example> GetAll(CancellationToken cancellationToken)
    {
        var values = await _exampleDataAdapter.GetValues(cancellationToken);

        var response = new Example
        {
            Names = [values.FirstValue, values.SecondValue]
        };
        return response;
    }

    /// <summary>
    /// An example business logic operation that checks permissions from the Entitlements Service.
    /// </summary>
    public async Task<Example> GetAllWithPermissionsCheckAsync(CancellationToken cancellationToken)
    {
        if (_userPermissions.UserHasPermission(AllocationManagement.Rates.View))
        {
            var values = await _exampleDataAdapter.GetValues(cancellationToken);
            var response = new Example()
            {
                Names = [values.FirstValue, values.SecondValue]
            };

            return response;
        }

        throw new Core.WebService.Contracts.Exceptions.ForbiddenV2Exception();
    }

    /// <summary>
    /// An example business logic operation.
    /// </summary>
    /// <returns></returns>
    public Example GetContextData(string authenticationUserId, string environmentId)
    {
        var response = new Example
        {
            Names = [authenticationUserId, environmentId]
        };
        return response;
    }
}