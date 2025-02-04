using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// Validator for product stock update request
/// </summary>
public sealed class UpdateStockRequestValidator : AbstractValidator<UpdateStockRequest>
{
    /// <summary>
    /// Initializes validation rules for product stock update
    /// </summary>
    public UpdateStockRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID is required");

        RuleFor(x => x.StockQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative");
    }
} 