using Blackbaud.UserInfo.Service.Controllers;
using Blackbaud.UserInfo.Service.DataAccess;
using Blackbaud.Testing;
using Blackbaud.Testing.Extensions;
using Moq;
using System;
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
public class ExampleControllerTests : IClassFixture<TestServerFixture>, IDisposable
{
    private readonly TestServerFixture _testServerFixture;

    public ExampleControllerTests(TestServerFixture testServerFixture)
    {
        _testServerFixture = testServerFixture;
    }

    public void Dispose()
    {
        _testServerFixture.GetMock<IExampleDataAdapter>().Reset();
    }

    #region /v1/example/values

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
        using var client = _testServerFixture.GetHttpClient();
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values")
            .ExpectResponse(new ExpectedHttpResponse<Models.Example>
            {
                Body = new()
                {
                    Names = ["first", "second"]
                }
            });
        await tester.SendAndValidate(client);
    }

    #endregion /v1/example/values

    #region /v1/example/values2

    [Fact]
    public async Task GetFromBrowserOrTrustedService200()
    {
        var bbid = Guid.NewGuid().ToString();
        var envId = "t-zU8eqz_2RUaGr63qKaNlyA";
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDAuthorization(
                envId,
                [(int)Permissions.Resolver.PermissionFlags.Usermanagement.Usermanagement.Users.View],
                bbid
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, $"/v1/example/values2?envid={envId}")
            .ExpectResponse(new ExpectedHttpResponse<Models.Example>
            {
                Body = new()
                {
                    Names = [bbid, envId]
                }
            });
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetFromBrowserOrTrustedService400MissingAuthUserId()
    {
        var envId = "t-zU8eqz_2RUaGr63qKaNlyA";
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDAuthorization(
                envId,
                [(int)Permissions.Resolver.PermissionFlags.Usermanagement.Usermanagement.Users.View],
                null
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, $"/v1/example/values2?envid={envId}")
            .ExpectResponse(new ExpectedInvalidInputV2ExceptionResponse("urn:blackbaud:model-validation-error", "Authentication User Id was not supplied."));
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetFromBrowserOrTrustedService400MissingEnvId()
    {
        var bbid = Guid.NewGuid().ToString();
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDAuthorization(
                null,
                [(int)Permissions.Resolver.PermissionFlags.Usermanagement.Usermanagement.Users.View],
                bbid
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values2")
            .ExpectResponse(new ExpectedInvalidInputV2ExceptionResponse("urn:blackbaud:model-validation-error", "Environment Id was not supplied."));
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetFromBrowserOrTrustedService401()
    {
        var envId = "t-zU8eqz_2RUaGr63qKaNlyA";
        using var client = _testServerFixture.GetHttpClient();
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, $"/v1/example/values2?envid={envId}")
            .ExpectNoResponseBody(HttpStatusCode.Unauthorized);
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetFromBrowserOrTrustedService403()
    {
        var bbid = Guid.NewGuid().ToString();
        var envId = "t-zU8eqz_2RUaGr63qKaNlyA";
        using var client = _testServerFixture.GetHttpClient()
            .WithBBIDAuthorization(
                envId,
                [(int)Permissions.Resolver.PermissionFlags.Generalledger.FENXT.Allocationmanagement.Allocations.View],
                bbid
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, $"/v1/example/values2?envid={envId}")
            .ExpectNoResponseBody(HttpStatusCode.Forbidden);
        await tester.SendAndValidate(client);
    }

    #endregion /v1/example/values2

    #region /v1/example/values3

    [Fact]
    public async Task GetSas200BBID()
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
            .WithBBIDAuthorization(
                permissions: [(int)Permissions.Resolver.PermissionFlags.Generalledger.FENXT.Allocationmanagement.Rates.View]
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values3")
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
    public async Task GetSas200SAS()
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
            .WithSASAuthorization(
                permissions: [(int)Permissions.Resolver.PermissionFlags.Generalledger.FENXT.Allocationmanagement.Rates.View],
                scopes: [ExampleController.SAS_SCOPE_ADVANCED_ACCESS]
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values3")
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
    public async Task GetSas401SAS_InsufficientScope()
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
            .WithSASAuthorization(
                [(int)Permissions.Resolver.PermissionFlags.Global.User.Administrator.BlackbaudEmployee]
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values3")
            .ExpectStatusWithoutValidatingResponse(HttpStatusCode.Unauthorized);
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetSas401()
    {
        using var client = _testServerFixture.GetHttpClient();
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values3")
            .ExpectNoResponseBody(HttpStatusCode.Unauthorized);
        await tester.SendAndValidate(client);
    }

    [Fact]
    public async Task GetSas403()
    {
        using var fixture = new TestServerFixture();
        using var client = fixture.GetHttpClient()
            .WithSASAuthorization(
                permissions: [(int)Permissions.Resolver.PermissionFlags.Usermanagement.Usermanagement.Users.View],
                scopes: [ExampleController.SAS_SCOPE_ADVANCED_ACCESS]
            );
        using var tester = new HttpRequestTester();
        tester
            .WithRequest(HttpMethod.Get, "/v1/example/values3")
            .ExpectResponse(new ExpectedForbiddenV2ExceptionResponse());
        await tester.SendAndValidate(client);
    }

    #endregion /v1/example/values3
}