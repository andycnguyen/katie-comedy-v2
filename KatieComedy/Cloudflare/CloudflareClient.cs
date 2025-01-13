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
    public required bool Success { get; init; }

    [JsonPropertyName("error-codes")]
    public required IReadOnlyList<string> ErrorCodes { get; init; }

    [JsonPropertyName("challenge_ts")]
    public required DateTimeOffset ChallengeTimestamp { get; init; }

    [JsonPropertyName("hostname")]
    public required string Hostname { get; init; }
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

        if (!response.IsSuccessStatusCode)
        {
            return new()
            {
                Success = false,
                ErrorCodes = [response.StatusCode.ToString()],
                ChallengeTimestamp = DateTimeOffset.UtcNow,
                Hostname = httpContextAccessor.HttpContext?.Request.Host.ToString() ?? string.Empty
            };
        }

        var result = await response.Content.ReadFromJsonAsync<SiteVerifyResult>(cancel);
        return result ?? throw new Exception("Invalid SiteVerify response.");
    }
}
