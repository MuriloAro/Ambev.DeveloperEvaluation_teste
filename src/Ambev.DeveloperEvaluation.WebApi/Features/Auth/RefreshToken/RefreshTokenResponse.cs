namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshToken;

/// <summary>
/// Response model for refresh token operation
/// </summary>
public sealed class RefreshTokenResponse
{
    /// <summary>
    /// Gets or sets the new JWT access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the new refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expiration date of the access token
    /// </summary>
    public DateTime AccessTokenExpiresAt { get; set; }
} 