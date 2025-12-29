using Microsoft.AspNetCore.Diagnostics;
using Blackbaud.UserInfo.Service.Storage;
using Blackbaud.UserInfo.Service.Messaging;
using Blackbaud.UserInfo.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Blackbaud.UserInfo.Service.Extensions
{
    /// <summary>
    /// Extension methods for configuring services in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds application-specific services to the dependency injection container.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection for method chaining.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            #region Storage

            services.AddUserBlobStorage();
            services.AddUserCosmosStorage();

            #endregion Storage

            #region Services Layer

            services.AddSingleton<IUserCosmosService, UserCosmosService>();
            services.AddScoped<IUserSupportalService, UserSupportalService>();
            //services.AddScoped<IImageCleanupService, ImageCleanupService>();
            //services.AddScoped<IImageLifeCycleService, ImageLifeCycleService>();

            #endregion Services Layer

            #region Other Services

            //services.AddScoped<IImageProcessor, ImageProcessor>();
            //services.AddSingleton<IExceptionHandler, ExceptionHandler>();
            //services.AddSingleton<IExceptionDomainCodeRegistry, ExceptionDomainCodeRegistry>();

            #endregion Other Services

            #region Service Bus

            services.AddServiceBusTopics();
            services.AddPublishers();

            #endregion Service Bus

            return services;
        }
    }
}
