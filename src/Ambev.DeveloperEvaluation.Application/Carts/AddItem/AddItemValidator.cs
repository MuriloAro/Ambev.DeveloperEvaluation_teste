using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.AddItem;

/// <summary>
/// Validator for cart item addition command
/// </summary>
public sealed class AddItemValidator : AbstractValidator<AddItemCommand>
{
    /// <summary>
    /// Initializes validation rules for cart item addition
    /// </summary>
    public AddItemValidator()
    {
        RuleFor(x => x.CartId)
            .NotEmpty()
            .WithMessage("Cart ID is required");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than 0");
    }
} 