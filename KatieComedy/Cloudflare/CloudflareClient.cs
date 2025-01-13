using System.Text.Json.Serialization;

namespace KatieComedy.Web.Cloudflare;

public record SiteVerifyRequest
{
    [JsonPropertyName("secret")]
    public required string Secret { get; init; }

    [JsonPropertyName("response")]
    public required string Token { get; init; }

    [JsonPropertyName("remoteip")]
    public required string RemoteIp { get; init; }
}

public record SiteVerifyResult
{
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    [JsonPropertyName("error-codes")]
    public IReadOnlyList<string> ErrorCodes { get; init; } = [];

    [JsonPropertyName("challenge_ts")]
    public DateTimeOffset? ChallengeTimestamp { get; init; }

    [JsonPropertyName("hostname")]
    public string? Hostname { get; init; }
}

public class CloudflareClient(
    IHttpClientFactory httpClientFactory,
    IHttpContextAccessor httpContextAccessor,
    IOptions<CloudflareOptions> options)
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    private readonly CloudflareOptions _options = options.Value;

    public async Task<SiteVerifyResult> ValidateToken(string token, CancellationToken cancel)
    {
        var request = new SiteVerifyRequest
        {
            Secret = _options.TurnstileSecretKey,
            Token = token,
            RemoteIp = httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty
        };

        var response = await _httpClient.PostAsJsonAsync(_options.SiteVerifyUrl, request, cancel);
        var result = await response.Content.ReadFromJsonAsync<SiteVerifyResult>(cancel);

        return result ?? new();
    }
}
