using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Validator for stock update command
/// </summary>
public sealed class UpdateStockValidator : AbstractValidator<UpdateStockCommand>
{
    /// <summary>
    /// Initializes validation rules for stock update
    /// </summary>
    public UpdateStockValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative");
    }
} 