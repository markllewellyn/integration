public class KeyVaultService : IKeyVaultService
{
    private readonly SecretClient? _secretClient;
    private readonly ILogger<KeyVaultService> _logger;
    private readonly string _environment;

    public KeyVaultService(IConfiguration configuration, ILogger<KeyVaultService> logger)
    {
        _logger = logger;

        var keyVaultUrl = configuration["KeyVaultUrl"];
        _environment = Environment.GetEnvironmentVariable("Environment") ?? "Development";

        if (string.IsNullOrWhiteSpace(keyVaultUrl))
            throw new InvalidOperationException("KeyVaultUrl is not configured.");

        if (_environment != "Development")
        {
            _secretClient = new SecretClient(
                new Uri(keyVaultUrl),
                new DefaultAzureCredential()
            );
        }
    }

    public async Task<string?> GetSecretAsync(string secretName)
    {
        if (string.IsNullOrWhiteSpace(secretName))
            throw new ArgumentException("Secret name is required.", nameof(secretName));

        if (_environment == "Development")
            return await GetLocalSecretAsync(secretName);

        var secret = await _secretClient!.GetSecretAsync(secretName);
        return secret.Value.Value;
    }

    private async Task<string?> GetLocalSecretAsync(string secretName)
    {
        var filePath = "keyvault.json";

        if (!File.Exists(filePath))
        {
            _logger.LogWarning("Local keyvault file not found.");
            return null;
        }

        var json = await File.ReadAllTextAsync(filePath);
        var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

        return secrets != null && secrets.TryGetValue(secretName, out var value)
            ? value
            : null;
    }
}
