using Blackbaud.Core.WebService.AspNetCore.Authorization;
using Blackbaud.Core.WebService.AspNetCore.Permissions;
using Blackbaud.Permissions.Resolver.PermissionFlags.Usermanagement.Usermanagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Blackbaud.UserInfo.Service.Authorization;

/// <summary>
/// Authorization policy name constants and extension methods for adding policies to the <see cref="IServiceCollection"/>
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    /// Example policy which enforces that the request has the base Users.View permission.
    /// See the below call to `ConfigurePolicy` which registers a policy by this string constant name.
    /// 
    /// Supportal routes can be controlled using supportal permissions teams can manage
    /// https://docs.blackbaud.com/engineering-system-docs/learn/supportals/permissions
    /// </summary>
    public const string UsersViewOnly = "UsersViewOnly";

    /// <summary>
    /// Example policy which enforces that the request is a team owner
    /// Whitelists can be used by teams to control supportal endpoints
    /// </summary>
    public const string EmpoweredUser = "EmpoweredUser";

    /// <summary>
    /// Example list of team members to use to control certain supportal routes
    /// </summary>
    internal static readonly HashSet<string> EmpoweredUsers = new(StringComparer.OrdinalIgnoreCase)
        {
            /* Team name */
            "c6829436-7445-4cda-9a87-ec372ca4cb43", // First.Last@blackbaud.com
            "dfd78f0b-5b20-47c5-8347-1b53ca2d4c88", // First.Last@blackbaud.com
        };

    /// <summary>
    /// Add authorization policies so they can be referenced from <see cref="AuthorizeAttribute"/>s
    /// </summary>
    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddAuthorization(authOptions =>
        {
            authOptions
                .ConfigurePolicy(UsersViewOnly, requestContext => requestContext.HasPermission(Users.View))
                .AddPolicy(EmpoweredUser, policyBuilder =>
                {
                    policyBuilder.Requirements.Add(new AuthenticationUserIdWhitelistRequirement(EmpoweredUsers));
                });
        });
    }
}