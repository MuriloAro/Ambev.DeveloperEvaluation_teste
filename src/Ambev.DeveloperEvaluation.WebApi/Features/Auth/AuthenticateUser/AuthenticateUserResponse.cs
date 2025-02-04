using System;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;

/// <summary>
/// Represents the response returned after successful authentication
/// </summary>
public sealed class AuthenticateUserResponse
{
    /// <summary>
    /// Gets or sets the JWT access token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets when the access token expires
    /// </summary>
    public DateTime AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the user's email
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's role
    /// </summary>
    public string Role { get; set; } = string.Empty;
}
