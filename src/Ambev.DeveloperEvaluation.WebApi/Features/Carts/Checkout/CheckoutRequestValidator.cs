using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Checkout;

/// <summary>
/// Validator for cart checkout request
/// </summary>
public sealed class CheckoutRequestValidator : AbstractValidator<CheckoutRequest>
{
    /// <summary>
    /// Initializes validation rules for cart checkout
    /// </summary>
    public CheckoutRequestValidator()
    {
        RuleFor(x => x.BranchId)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
} 