using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Checkout;

/// <summary>
/// Validator for cart checkout command
/// </summary>
public sealed class CheckoutValidator : AbstractValidator<CheckoutCommand>
{
    /// <summary>
    /// Initializes validation rules for cart checkout
    /// </summary>
    public CheckoutValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart ID is required");

        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
} 