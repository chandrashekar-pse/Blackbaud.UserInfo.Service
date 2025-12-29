using Blackbaud.Blob.Storage.Extensions;
using Blackbaud.Cosmos;
using Blackbaud.UserInfo.Service.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Blackbaud.UserInfo.Service.Storage
{
    /// <summary>
    /// Service collection extensions for registering image storage services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register Blob Storage services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserBlobStorage(this IServiceCollection services)
        {
            services.AddBlackbaudBlobStorage();
            services.AddBlackbaudStorageAccount<UserBlobStorageAccountConfig>();
            //services.AddScoped<IImageBlobRepository, ImageBlobRepository>();

            return services;
        }

        /// <summary>
        /// Register Cosmos DB Storage services
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddUserCosmosStorage(this IServiceCollection services)
        {
            services.AddSingleton<IUserCosmosRepository,  UserCosmosRepository>();
            services.AddSingleton<IUserDataAdapter, UserDataAdapter>();
            services.AddSingleton<IUserCosmosDbService,   UserCosmosDbService>();
            services.AddSingleton<CosmosClientConfiguration>();
            services.AddCosmosClient("nos-db");
            services.AddHostedService<CosmosDatabaseInitialisationStartupTask<IUserCosmosDbService>>();
            return services;
        }
    }
}
