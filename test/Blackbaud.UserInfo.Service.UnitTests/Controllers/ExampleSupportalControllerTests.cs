using Blackbaud.UserInfo.Service.Authorization;
using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.Testing;
using Blackbaud.Testing.Extensions;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Blackbaud.UserInfo.Service.UnitTests;

/// <summary>
/// IClassFixture provides an efficient way for all tests in the class to share the same instance of TestServerFixture to improve performance
/// To avoid tests interfering with each other, reset mocks between tests as needed
/// </summary>
public class ExampleSupportalControllerTests : IClassFixture<TestServerFixture>, IDisposable
{
    private readonly TestServerFixture _testServerFixture;

    public ExampleSupportalControllerTests(TestServerFixture testServerFixture)
    {
        _testServerFixture = testServerFixture;
    }

    public void Dispose()
    {
        _testServerFixture.GetMock<IExampleDataAdapter>().Reset();
    }

    #region /v1/supportal/values

    [Fact]
    public async Task GetValues200()
    {
        _testServerFixture.GetMock<IExampleDataAdapter>().Setup(m => m.GetValues(It.IsAny<CancellationToken>())).ReturnsAsync(() =>
        {
            return new()
            {
                FirstValue = "first",
                SecondValue = "second"
            };
        });
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDBlackbaudEmployeeAuthorization();
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/supportal/values")
            .ExpectResponse(new ExpectedHttpResponse<Models.Example>
            {
                Body = new()
                {
                    Names = ["first", "second"]
                }
            });
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetValues401()
    {
        _testServerFixture.GetMock<IExampleDataAdapter>().Setup(m => m.GetValues(It.IsAny<CancellationToken>())).ReturnsAsync(() =>
        {
            return new()
            {
                FirstValue = "first",
                SecondValue = "second"
            };
        });
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDAuthorization();
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/supportal/values")
            .ExpectStatusWithoutValidatingResponse(HttpStatusCode.Unauthorized);
        await tester.SendAndValidate(client);
    }

    #endregion /v1/supportal/values

    #region /v1/supportal/values/detailed

    [Fact]
    public async Task GetValuesDetailed200()
    {
        _testServerFixture.GetMock<IExampleDataAdapter>().Setup(m => m.GetValues(It.IsAny<CancellationToken>())).ReturnsAsync(() =>
        {
            return new()
            {
                FirstValue = "first",
                SecondValue = "second"
            };
        });
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDBlackbaudEmployeeAuthorization(bbid: AuthorizationPolicies.EmpoweredUsers.First());
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/supportal/values/detailed")
            .ExpectResponse(new ExpectedHttpResponse<Models.Example>
            {
                Body = new()
                {
                    Names = ["first", "second"]
                }
            });
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetValuesDetailed403()
    {
        _testServerFixture.GetMock<IExampleDataAdapter>().Setup(m => m.GetValues(It.IsAny<CancellationToken>())).ReturnsAsync(() =>
        {
            return new()
            {
                FirstValue = "first",
                SecondValue = "second"
            };
        });
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDBlackbaudEmployeeAuthorization();
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/supportal/values/detailed")
            .ExpectStatusWithoutValidatingResponse(HttpStatusCode.Forbidden);
        await tester.SendAndValidate(client);
    }

    #endregion /v1/supportal/values/detailed
}