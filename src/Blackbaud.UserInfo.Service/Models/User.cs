using System;

namespace Blackbaud.UserInfo.Service.Models
{
    /// <summary>
    /// Base class for cosmos user reference document
    /// </summary>
    public class User
    {
        /// <summary>
        /// The cosmos unique identifier (required)
        /// </summary>
        public required string id { get; set; } = Guid.NewGuid().ToString(); // GUID

        /// <summary>
        /// First name of the user.
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        public int? Phone { get; init; }

        /// <summary>
        /// Entity ID GUID
        /// </summary>
        public required Guid EntityId { get; set; } // GUID & PartitionKey
    }
}
