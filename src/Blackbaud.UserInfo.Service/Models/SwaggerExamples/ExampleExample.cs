using Swashbuckle.AspNetCore.Filters;

namespace Blackbaud.UserInfo.Service.Models.SwaggerExamples;

/// <summary>
/// Swagger example for <see cref="Example"/>.
/// </summary>
public class ExampleExample : IExamplesProvider<Example>
{
    /// <inheritdoc/>
    public Example GetExamples()
    {
        return new()
        {
            Names = ["John", "Paul", "George", "Ringo"]
        };
    }
}