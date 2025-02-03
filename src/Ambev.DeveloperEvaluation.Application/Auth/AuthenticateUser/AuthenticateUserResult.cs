namespace Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;

/// <summary>
/// Represents the response after authenticating a user
/// </summary>
public sealed class AuthenticateUserResult
{
    /// <summary>
    /// Gets or sets the authentication token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the refresh token
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the expiration date of the access token
    /// </summary>
    public DateTime AccessTokenExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets the user's email address
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
