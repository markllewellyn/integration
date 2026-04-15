public class CosmosDbService : ICosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly ILogger<CosmosDbService> _logger;

    private const string DatabaseId = "AppDatabase";
    private const string ContainerId = "Tenants";

    public CosmosDbService(CosmosClient cosmosClient, ILogger<CosmosDbService> logger)
    {
        _cosmosClient = cosmosClient;
        _logger = logger;
    }

    public async Task<List<TenantConfig>> GetActiveTenantsAsync(CancellationToken cancellationToken)
    {
        var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);

        var query = new QueryDefinition("SELECT * FROM c WHERE c.Status = @status")
            .WithParameter("@status", "active");

        var iterator = container.GetItemQueryIterator<TenantConfig>(query);

        var tenants = new List<TenantConfig>();

        while (iterator.HasMoreResults)
        {
            try
            {
                var response = await iterator.ReadNextAsync(cancellationToken);
                tenants.AddRange(response.Resource);
            }
            catch (CosmosException ex)
            {
                _logger.LogError(ex, "Error retrieving tenants");
                throw;
            }
        }

        return tenants;
    }

    public async Task UpdateTenantAsync(string tenantId, Action<TenantConfig> update, CancellationToken cancellationToken)
    {
        var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);

        var query = new QueryDefinition("SELECT * FROM c WHERE c.TenantId = @tenantId")
            .WithParameter("@tenantId", tenantId);

        var iterator = container.GetItemQueryIterator<TenantConfig>(query);
        var response = await iterator.ReadNextAsync(cancellationToken);

        var tenant = response.Resource.FirstOrDefault();

        if (tenant == null)
        {
            _logger.LogWarning("Tenant {TenantId} not found", tenantId);
            throw new InvalidOperationException($"Tenant {tenantId} not found");
        }

        update(tenant);

        await container.UpsertItemAsync(
            tenant,
            new PartitionKey(tenant.TenantId),
            cancellationToken: cancellationToken);

        _logger.LogInformation("Tenant {TenantId} updated successfully", tenantId);
    }
}
