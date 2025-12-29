namespace Blackbaud.UserInfo.Service.DataAccess.Models;

/// <summary>
/// An example data model
/// </summary>
public record Example
{
    /// <summary>
    /// First value from the Data Store
    /// </summary>
    public required string FirstValue { get; init; }

    /// <summary>
    /// Second value from the Data Store
    /// </summary>
    public required string SecondValue { get; init; }
}