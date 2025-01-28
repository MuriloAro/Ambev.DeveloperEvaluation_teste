using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(100);

        RuleFor(p => p.Description)
            .MaximumLength(500);

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .PrecisionScale(10, 2, false);

        RuleFor(p => p.StockQuantity)
            .GreaterThanOrEqualTo(0);

        RuleFor(p => p.Status)
            .NotEqual(ProductStatus.Unknown)
            .WithMessage("Product status cannot be Unknown");

        RuleFor(p => p.Category)
            .NotEqual(ProductCategory.None)
            .WithMessage("Product category must be specified");
    }
} 