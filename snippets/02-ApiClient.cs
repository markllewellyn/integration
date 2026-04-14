public interface IOrderApiClient
{
    Task<List<Order>> GetOrdersAsync(TenantConfig tenant);
}

public class MockOrderApiClient : IOrderApiClient
{
    public Task<List<Order>> GetOrdersAsync(TenantConfig tenant)
    {
        // Simulated external API response
        var orders = new List<Order>
        {
            new Order { Id = $"{tenant.Name}-001", Amount = 120 },
            new Order { Id = $"{tenant.Name}-002", Amount = 250 }
        };

        return Task.FromResult(orders);
    }
}
