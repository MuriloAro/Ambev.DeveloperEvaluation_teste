using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.RemoveItem;

/// <summary>
/// Validator for cart item removal request
/// </summary>
public sealed class RemoveItemRequestValidator : AbstractValidator<RemoveItemRequest>
{
    /// <summary>
    /// Initializes validation rules for cart item removal
    /// </summary>
    public RemoveItemRequestValidator()
    {
        RuleFor(x => x.ItemId)
            .NotEmpty()
            .WithMessage("Cart item ID is required");
    }
} 