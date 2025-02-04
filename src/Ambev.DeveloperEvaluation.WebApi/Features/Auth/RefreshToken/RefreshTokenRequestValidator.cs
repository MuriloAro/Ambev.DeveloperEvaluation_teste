using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth.RefreshToken;

/// <summary>
/// Validator for refresh token request
/// </summary>
public sealed class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    /// <summary>
    /// Initializes validation rules for refresh token request
    /// </summary>
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required");
    }
} 