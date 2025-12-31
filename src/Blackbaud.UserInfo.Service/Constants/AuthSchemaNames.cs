namespace Blackbaud.UserInfo.Service.Constants
{
    /// <summary>
    /// Centralized constants for authentication scheme names used throughout the service.
    /// </summary>
    public static class AuthSchemeNames
    {
        /// <summary>
        /// SAS authentication scheme.
        /// </summary>
        public const string Sas = "SAS";

        /// <summary>
        /// BBID authentication scheme.
        /// </summary>
        public const string Bbid = "BBID";

        /// <summary>
        /// Authentication scheme that accepts either a SAS token or a BBID (Blackbaud ID).
        /// Use this scheme when requests may be authenticated by either mechanism.
        /// </summary>
        public const string SasOrBbid = "SAS,BBID";
    }
}
