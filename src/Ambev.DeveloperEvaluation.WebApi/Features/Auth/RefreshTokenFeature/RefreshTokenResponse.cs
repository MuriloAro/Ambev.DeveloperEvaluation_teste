namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshTokenFeature;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
} 