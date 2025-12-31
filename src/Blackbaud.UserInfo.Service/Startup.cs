using Azure.Identity;
using Blackbaud.Core.WebService.AspNetCore;
using Blackbaud.Core.WebService.Contracts;
using Blackbaud.UserInfo.Service.Authorization;
using Blackbaud.UserInfo.Service.Extensions;
using Blackbaud.UserInfo.Service.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blackbaud.UserInfo.Service;

/// <summary>
/// Startup class for configuring the web service.
/// </summary>
public class Startup
{
    private IConfiguration Configuration { get; }

    /// <summary>
    /// Constructor to inject Configuration
    /// </summary>
    /// <param name="configuration"></param>
    public Startup(IConfiguration configuration) => Configuration = configuration;

    /// <summary>
    /// Configure dependency injection container
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        // Configure an example Business Logic class
        services.AddSingleton<DataAccess.IExampleDataAdapter>(
            _ => new DataAccess.ExampleDataAdapter()
        );
        // Configure an example Business Logic class
        services.AddScoped((serviceProvider) =>
            {
                var userPermissions = serviceProvider.GetService<Permissions.Resolver.UserPermissions>();
                var dataAdapter = serviceProvider.GetService<DataAccess.IExampleDataAdapter>();

                return new BusinessLogic.ExampleService(
                    userPermissions,
                    dataAdapter
                );
            })
            .AddScoped(provider => new Permissions.Resolver.UserPermissions(
                provider.GetService<IRequestContext>()?.UserAccess ?? new UserAccess()
            ));

        services.AddMonitorTest<MonitorTests.ExampleMonitorTest>();
        services.AddServices();
        services.AddAuthorizationPolicies();

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen();

        // Bind Key Vault (RBAC via Managed Identity) 
        // Core.App helper: register SecretClient from ES-injected vault URI + MI 
        // e.g. services.AddKeyVaultSecretClientFromEngineeringConfig(); 

        //services.AddSingleton<ISecretProvider, KeyVaultSecretProvider>();

        //// Register SecretClient with DefaultAzureCredential (Managed Identity first)
        //var keyVaultName = Configuration["KeyVault:Name"] ?? "KV_NAME";
        //var kvUri = new Uri($"https://{keyVaultName}.vault.azure.net");

        //var credential = new DefaultAzureCredential();
        //var secretClient = new SecretClient(kvUri, credential);

        //services.AddSingleton(secretClient);
    }

    /// <summary>
    /// Configure ASP.NET Core MVC application
    /// </summary>
    /// <param name="app"></param>
    public void Configure(IApplicationBuilder app) => app.ConfigureBlackbaud<Startup>();

    //public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
    //                          IHostApplicationLifetime lifetime, SecretClient client)
    //{
    //    if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

    //    app.UseRouting();
    //    app.UseEndpoints(endpoints => endpoints.MapControllers());

    //    // Kick off your original workflow on app started (fire-and-forget with proper logging).
    //    lifetime.ApplicationStarted.Register(() => _ = RunKeyVaultWorkflowAsync(client));
    //}

    //private static async Task RunKeyVaultWorkflowAsync(SecretClient client)
    //{
    //    const string secretName = "SECRET_NAME";
    //    var secretValue = "SECRET_VALUE";

    //    Console.Write($"Creating a secret called '{secretName}' with the value '{secretValue}' ...");
    //    await client.SetSecretAsync(secretName, secretValue);
    //    Console.WriteLine(" done.");

    //    Console.WriteLine("Forgetting your secret.");
    //    secretValue = string.Empty;
    //    Console.WriteLine($"Your secret is '{secretValue}'.");

    //    Console.WriteLine("Retrieving your secret.");
    //    var secret = await client.GetSecretAsync(secretName);
    //    Console.WriteLine($"Your secret is '{secret.Value.Value}'.");

    //    Console.Write($"Deleting your secret ...");
    //    DeleteSecretOperation operation = await client.StartDeleteSecretAsync(secretName);
    //    await operation.WaitForCompletionAsync();
    //    Console.WriteLine(" done.");

    //    Console.Write($"Purging your secret ...");
    //    await client.PurgeDeletedSecretAsync(secretName);
    //    Console.WriteLine(" done.");
    //}

}