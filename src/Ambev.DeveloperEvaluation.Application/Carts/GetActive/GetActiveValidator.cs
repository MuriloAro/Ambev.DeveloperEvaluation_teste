using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetActive;

/// <summary>
/// Validator for retrieving active cart command
/// </summary>
public sealed class GetActiveValidator : AbstractValidator<GetActiveCommand>
{
    /// <summary>
    /// Initializes validation rules for retrieving active cart
    /// </summary>
    public GetActiveValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
} 