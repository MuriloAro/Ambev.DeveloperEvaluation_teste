using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.RemoveItem;

/// <summary>
/// Validator for cart item removal command
/// </summary>
public sealed class RemoveItemValidator : AbstractValidator<RemoveItemCommand>
{
    /// <summary>
    /// Initializes validation rules for cart item removal
    /// </summary>
    public RemoveItemValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
} 