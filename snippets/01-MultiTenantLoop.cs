foreach (var tenant in tenants)
{
    var orders = await _apiClient.GetOrdersAsync(tenant);

    foreach (var order in orders)
    {
        var report = Transform(order);
        await _repository.SaveAsync(report);
    }
}
