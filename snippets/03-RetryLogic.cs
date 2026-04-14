public async Task<List<Order>> GetOrdersWithRetry(TenantConfig tenant, ILogger log)
{
    const int maxRetries = 3;

    for (int attempt = 1; attempt <= maxRetries; attempt++)
    {
        try
        {
            return await _apiClient.GetOrdersAsync(tenant);
        }
        catch (Exception ex)
        {
            log.LogWarning($"Attempt {attempt} failed for {tenant.Name}: {ex.Message}");

            if (attempt == maxRetries)
                throw;

            await Task.Delay(1000);
        }
    }

    return new List<Order>();
}
