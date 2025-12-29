#pragma warning disable // Disable all warnings
using System;
namespace Blackbaud.Permissions.Resolver.PermissionFlags.Usermanagement.Usermanagement
{
    public enum Users
    {
        MarkInactive = 17,

        View = 15,

        AddEdit = 16,

        Delete = 3294,
    }
}

namespace Blackbaud.Permissions.Resolver.PermissionFlags.Generalledger.FENXT.Allocationmanagement
{
    public enum Rates
    {
        View = 2193,

        Add = 2194,

        Edit = 2195,

        Delete = 2196,
    }

    public enum Allocations
    {
        View = 2189,

        Add = 2190,

        Edit = 2191,

        Delete = 2192,
    }
}

namespace Blackbaud.Permissions.Resolver.PermissionFlags.Global.User
{
    public enum Administrator
    {
        BlackbaudEmployee = 1,

        LegalEntity = 2,

        [Obsolete("Use granular permissions")]
        Environment = 3,
    }
}
