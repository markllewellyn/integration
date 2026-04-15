[FunctionName("GetOrders")]
public async Task GetOrders(
    [TimerTrigger("%GetOrdersSchedule%", RunOnStartup = true)] TimerInfo myTimer,
    CancellationToken cancellationToken)
{
    _logger.LogInformation("GetOrders function started");

    var tenants = await GetTenants(cancellationToken);

    foreach (var tenant in tenants)
    {
        var runStartedUtc = DateTime.UtcNow;

        // ---------------------------------------------
        // Calculate incremental sync window
        // ---------------------------------------------
        DateTime lastRun = DateTime.ParseExact(
            tenant.OrdersCreatedAfter,
            Constants.DateTimeWithSeconds,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AssumeLocal
        ).ToUniversalTime();

        DateTime runTo = runStartedUtc;

        var maxWindow = lastRun.AddHours(168);
        if ((runTo - lastRun) > TimeSpan.FromHours(168))
            runTo = maxWindow;

        if (runTo > DateTime.UtcNow.AddMinutes(-2))
            runTo = DateTime.UtcNow.AddMinutes(-2);

        _logger.LogInformation(
            $"Processing tenant {tenant.TenantID} from {tenant.OrdersCreatedAfter} to {runTo}"
        );

        var orders = await GetOrdersForTenant(tenant, runTo.ToString());

        if (orders == null || orders.Count == 0)
        {
            await UpdateTenant(tenant.TenantID, t =>
            {
                t.OrdersCreatedAfter = runTo.ToString();
            }, cancellationToken);

            continue;
        }

        var oracleDb = new OracleDbService(
            _logger,
            GetConnectionStringForTenant(tenant.TenantID)
        );

        foreach (var order in orders)
        {
            if (!await OrderExistsAsync(oracleDb, order.AmazonOrderId))
            {
                await ProcessOrderAsync(order, tenant, oracleDb);
            }
        }

        await UpdateTenant(tenant.TenantID, t =>
        {
            t.OrdersCreatedAfter = runTo.ToString();
        }, cancellationToken);
    }

    _logger.LogInformation("GetOrders function completed");
}
