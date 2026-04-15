[assembly: FunctionsStartup(typeof(Startup))]

namespace IntegrationDemo.Functions
{
    using Microsoft.Azure.Functions.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Azure.Cosmos;
    using MarketPlaceGateway.CosmosDatabase.Repository;
    using MarketPlaceGateway.Api.Services;
    using MarketPlaceGateway.Core.Services;
    using MarketPlaceGateway.OracleDatabase.Repository;
    using static MarketPlaceGateway.Core.Services.Enums;

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var configuration = builder.GetContext().Configuration;

            builder.Services.AddSingleton(configuration);
            builder.Services.AddLogging();

            var cosmosConnection = configuration["CosmosDbConnectionString"];

            // -----------------------------------------
            // Tenant storage strategy decision
            // -----------------------------------------
            if (!string.IsNullOrEmpty(cosmosConnection))
            {
                builder.Services.AddSingleton(_ => new CosmosClient(cosmosConnection));
                builder.Services.AddSingleton<CosmosDbService>();
            }
            else
            {
                var tenantStrategySetting = configuration["TenantStrategy"];

                if (!Enum.TryParse(tenantStrategySetting, true, out TenantStrategy strategy))
                    strategy = TenantStrategy.BlobStorage;

                if (strategy == TenantStrategy.CosmosDb)
                    throw new InvalidOperationException("Cosmos DB connection string not configured.");
            }

            // -----------------------------------------
            // Core services
            // -----------------------------------------
            builder.Services.AddSingleton<IApiAuthenticator, ApiAuthenticator>();
            builder.Services.AddSingleton<IApiHelper, ApiHelper>();

            builder.Services.AddSingleton<IOrderFetch, OrderFetch>();
            builder.Services.AddSingleton<IAddressFetch, AddressFetch>();
            builder.Services.AddSingleton<IOrderItemFetch, OrderItemFetch>();

            builder.Services.AddSingleton<KeyVaultService>();

            // -----------------------------------------
            // Database services
            // -----------------------------------------
            builder.Services.AddSingleton(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<OracleDbService>>();
                return new OracleDbService(logger, "ConnectionString");
            });
        }
    }
}
