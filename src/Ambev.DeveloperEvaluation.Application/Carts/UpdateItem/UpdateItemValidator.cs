using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateItem;

/// <summary>
/// Validator for cart item update command
/// </summary>
public sealed class UpdateItemValidator : AbstractValidator<UpdateItemCommand>
{
    /// <summary>
    /// Initializes validation rules for cart item update
    /// </summary>
    public UpdateItemValidator()
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
    }
} 