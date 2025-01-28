using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(item => item.ProductId)
            .NotEmpty()
            .WithMessage("Product is required");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero")
            .LessThanOrEqualTo(20)
            .WithMessage("Cannot sell more than 20 identical items");

        RuleFor(item => item.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Unit price must be greater than zero")
            .PrecisionScale(10, 2, false)
            .WithMessage("Unit price cannot have more than 2 decimal places");

        RuleFor(item => item.TotalAmount)
            .GreaterThan(0)
            .WithMessage("Total amount must be greater than zero")
            .PrecisionScale(10, 2, false)
            .WithMessage("Total amount cannot have more than 2 decimal places");
    }
} 