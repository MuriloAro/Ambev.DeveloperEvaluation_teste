namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUser;

/// <summary>
/// Request model for user authentication
/// </summary>
public sealed class AuthenticateUserRequest
{
    /// <summary>
    /// Gets or sets the user's email address
    /// Must be a valid email format
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's password
    /// Must match the stored hashed password
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
