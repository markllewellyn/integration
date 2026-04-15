namespace IntegrationDemo.Services
{
    using IntegrationDemo.Models;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IConfiguration configuration, HttpClient httpClient, ILogger<TokenService> logger)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _logger = logger;
        }

        private string GetTenantConfig(string tenantId, string key)
        {
            return _configuration[$"{tenantId}_{key}"] ?? string.Empty;
        }

        public async Task<AccessToken> GetAccessTokenAsync(TenantConfig tenant)
        {
            var refreshToken = GetTenantConfig(tenant.TenantId, "refreshToken");
            var clientId = GetTenantConfig(tenant.TenantId, "clientId");
            var clientSecret = GetTenantConfig(tenant.TenantId, "clientSecret");

            var missingConfigs = new List<string>();
            if (string.IsNullOrEmpty(refreshToken)) missingConfigs.Add("refreshToken");
            if (string.IsNullOrEmpty(clientId)) missingConfigs.Add("clientId");
            if (string.IsNullOrEmpty(clientSecret)) missingConfigs.Add("clientSecret");

            if (missingConfigs.Any())
            {
                var missing = string.Join(", ", missingConfigs);
                _logger.LogWarning("Missing configuration for tenant {TenantId}: {Missing}", tenant.TenantId, missing);
                throw new InvalidOperationException($"Missing configuration for tenant {tenant.TenantId}: {missing}");
            }

            var requestBody = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };

            var content = new FormUrlEncodedContent(requestBody);

            try
            {
                var response = await _httpClient.PostAsync(_configuration["AuthEndpoint"], content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Token request failed for tenant {TenantId}. Status: {StatusCode}",
                        tenant.TenantId, response.StatusCode);

                    throw new InvalidOperationException($"Token request failed with status {response.StatusCode}");
                }

                var responseBody = await response.Content.ReadAsStringAsync();

                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
                    responseBody,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    _logger.LogWarning("Invalid token response for tenant {TenantId}. Response: {Response}",
                        tenant.TenantId, responseBody);

                    throw new InvalidOperationException("Invalid token response");
                }

                _logger.LogInformation("Successfully retrieved token for tenant {TenantId}", tenant.TenantId);

                return new AccessToken
                {
                    Token = tokenResponse.AccessToken,
                    Expiration = tokenResponse.ExpiresIn
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving token for tenant {TenantId}", tenant.TenantId);
                throw new Exception($"Authentication error for tenant {tenant.TenantId}", ex);
            }
        }
    }
}
