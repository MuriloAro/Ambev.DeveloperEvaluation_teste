using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(s => s.CustomerId)
            .NotEmpty()
            .WithMessage("Customer is required");

        RuleFor(s => s.BranchId)
            .NotEmpty()
            .WithMessage("Branch is required");

        RuleFor(s => s.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item");

        RuleForEach(s => s.Items).SetValidator(new SaleItemValidator());
    }
} 