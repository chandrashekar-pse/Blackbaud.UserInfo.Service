using System.Collections.Generic;

namespace Blackbaud.UserInfo.Service.Models;

/// <summary>
/// An example data model
/// </summary>
public record Example
{
    /// <summary>
    /// The example property
    /// </summary>
    public required List<string> Names { get; init; }
}