public async Task RunAsync(ILogger log)
{
    log.LogInformation("Starting multi-tenant processing");

    var tenants = await _tenantService.GetTenantsAsync();

    foreach (var tenant in tenants.Where(t => t.IsActive))
    {
        try
        {
            log.LogInformation($"Processing tenant: {tenant.Name}");

            var orders = await _apiClient.GetOrdersAsync(tenant);

            foreach (var order in orders)
            {
                var report = Transform(order, tenant);
                await _repository.SaveAsync(report);
            }
        }
        catch (Exception ex)
        {
            log.LogError(ex, $"Error processing tenant {tenant.Name}");
        }
    }

    log.LogInformation("Processing completed");
}
