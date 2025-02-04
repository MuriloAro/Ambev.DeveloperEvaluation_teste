using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItem;

/// <summary>
/// Validator for cart item addition request
/// </summary>
public sealed class AddItemRequestValidator : AbstractValidator<AddItemRequest>
{
    /// <summary>
    /// Initializes validation rules for cart item addition
    /// </summary>
    public AddItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
    }
} 