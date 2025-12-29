using Blackbaud.Core.WebService.AspNetCore;
using Blackbaud.UserInfo.Service.BusinessLogic;
using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.UserInfo.Service.Models;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Blackbaud.UserInfo.Service.UnitTests;

public class ExampleTest
{
    [Fact]
    public async Task ReturnCorrectModel()
    {
        var exampleDataAdapter = new Mock<IExampleDataAdapter>();
        var svc = new ExampleService(new(new UserAccess()), exampleDataAdapter.Object);
        exampleDataAdapter.Setup(r => r.GetValues(It.IsAny<CancellationToken>())).ReturnsAsync(() =>
        {
            return new()
            {
                FirstValue = "first",
                SecondValue = "second"
            };
        });

        var token = new CancellationToken();
        var expected = new Example()
        {
            Names = ["first", "second"]
        };
        var result = await svc.GetAll(token);
        Assert.Equal(expected.Names, result.Names);
    }
}