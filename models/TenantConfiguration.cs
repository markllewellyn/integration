public class TenantConfiguration
{
    // Identity
    public required string Id { get; set; }
    public required string TenantID { get; set; }
    public string? TenantName { get; set; }
    public string? Status { get; set; }

    // API Configuration
    public string? ApiUrl { get; set; }
    public string? OrdersEndPoint { get; set; }
    public string? AddressEndPoint { get; set; }
    public string? OrderItemsEndPoint { get; set; }
    public string? ReturnEndPoint { get; set; }
    public string? ReportEndpoint { get; set; }
    public string? FeedsUrl { get; set; }
    public string? DocumentsUrl { get; set; }

    // Marketplace Configuration
    public string? MarketPlaceId { get; set; }
    public string? MerchantId { get; set; }

    // Processing State
    public string? OrdersCreatedAfter { get; set; }
    public string? LastUpdatedAfter { get; set; }

    // Behaviour Controls
    public int ReturnReportLookbackDays { get; set; }
    public int ProcessingTimeoutMinutes { get; set; }
    public int CancellationTokenDelay { get; set; }

    // Execution Limits
    public int DefaultTimeoutSeconds { get; set; } = 180;
    public int MaxDelayMilliseconds { get; set; } = 30000;

    // Flags
    [JsonConverter(typeof(FlexibleBoolConverter))]
    public bool TestMode { get; set; }
}
