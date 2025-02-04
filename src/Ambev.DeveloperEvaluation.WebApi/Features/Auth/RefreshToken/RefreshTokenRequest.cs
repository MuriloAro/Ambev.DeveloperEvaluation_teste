namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshToken;

/// <summary>
/// Request model for refreshing access token
/// </summary>
public sealed class RefreshTokenRequest
{
    /// <summary>
    /// Gets or sets the refresh token used to obtain new access token
    /// Must be a valid and non-expired refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
} 