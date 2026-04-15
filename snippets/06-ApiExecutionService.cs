public async Task<RestResponse> ExecuteRequestAsync(
    string endpoint,
    TenantConfig tenant,
    Method httpMethod = Method.Get,
    string? body = null,
    Dictionary<string, string>? queryParams = null,
    Dictionary<string, string>? headers = null)
{
    if (tenant == null)
        throw new ArgumentNullException(nameof(tenant));

    if (!_limiters.TryGetValue(endpoint, out var limiter))
        throw new InvalidOperationException($"No rate limiter configured for endpoint '{endpoint}'");

    using var cts = new CancellationTokenSource(tenant.CancellationTokenDelay);

    // Acquire rate limit token
    while (!cts.Token.IsCancellationRequested)
    {
        var lease = await limiter.AcquireAsync(1, cts.Token);

        if (lease.IsAcquired)
            break;

        if (lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfter))
            await Task.Delay(retryAfter, cts.Token);
        else
            await Task.Delay(250, cts.Token);
    }

    var client = GetOrCreateClient(tenant.ApiUrl);
    var request = new RestRequest(endpoint, httpMethod);

    // Add query parameters
    if (queryParams != null)
    {
        foreach (var param in queryParams)
            request.AddQueryParameter(param.Key, param.Value);
    }

    // Add request body
    if (!string.IsNullOrEmpty(body) && (httpMethod == Method.Post || httpMethod == Method.Put))
    {
        request.AddStringBody(body, DataFormat.Json);
    }

    // Ensure valid token
    if (!await EnsureValidAccessTokenAsync(tenant))
        throw new Exception("Unable to obtain access token");

    var token = _tenantAccessTokens[tenant.TenantId].Token;

    request.AddHeader("Authorization", $"Bearer {token}");

    // Add custom headers
    if (headers != null)
    {
        foreach (var header in headers)
            request.AddOrUpdateHeader(header.Key, header.Value);
    }

    int retryCount = 0;
    int maxRetries = 5;

    while (retryCount < maxRetries)
    {
        try
        {
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                _logger.LogInformation("API request successful");
                return response;
            }

            // Handle rate limiting (429)
            if (response.StatusCode == HttpStatusCode.TooManyRequests)
            {
                var retryAfter = GetRetryAfterSeconds(response.Headers);
                await Task.Delay(TimeSpan.FromSeconds(retryAfter));
                retryCount++;
                continue;
            }

            // Handle transient errors
            if (response.StatusCode == 0 || response.ErrorException != null)
            {
                _logger.LogWarning("Transport error, retrying attempt {Retry}", retryCount + 1);
                retryCount++;
                await Task.Delay(1000 * (int)Math.Pow(2, retryCount));
                continue;
            }

            // Retry other transient status codes
            if (IsRetryableError(response.StatusCode))
            {
                retryCount++;
                await Task.Delay(1000 * (int)Math.Pow(2, retryCount));
                continue;
            }

            // Non-retryable
            _logger.LogError("Non-retryable API error: {StatusCode}", response.StatusCode);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during API request");

            if (retryCount < maxRetries)
            {
                retryCount++;
                await Task.Delay(1000 * (int)Math.Pow(2, retryCount));
                continue;
            }

            throw;
        }
    }

    _logger.LogError("Request failed after max retries");
    return new RestResponse { StatusCode = 0, ErrorMessage = "Max retries exceeded" };
}
