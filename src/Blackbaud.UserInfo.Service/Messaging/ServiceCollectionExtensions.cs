using Microsoft.Extensions.DependencyInjection;

namespace Blackbaud.UserInfo.Service.Messaging
{
    /// <summary>
    /// Extension methods for registering messaging-related services in the dependency injection container.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers service bus topic dependencies in the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection for method chaining.</returns>
        public static IServiceCollection AddServiceBusTopics(this IServiceCollection services)
        {
            services.AddBlackbaudServiceBus();
            return services;
        }

        /// <summary>
        /// Registers publisher services for messaging events in the service collection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns>The same service collection for method chaining.</returns>
        public static IServiceCollection AddPublishers(this IServiceCollection services)
        {
            //services.AddScoped<IImageCleanupEventPublisher, ImageCleanupEventPublisher>();
            return services;
        }
    }
}
