using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Validator for sale confirmation command
/// </summary>
public sealed class ConfirmSaleValidator : AbstractValidator<ConfirmSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for sale confirmation
    /// </summary>
    public ConfirmSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 