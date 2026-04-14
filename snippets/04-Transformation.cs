private OrderReport Transform(Order order, TenantConfig tenant)
{
    return new OrderReport
    {
        OrderId = order.Id,
        TenantName = tenant.Name,
        TotalAmount = order.Amount,
        ProcessedDate = DateTime.UtcNow
    };
}
