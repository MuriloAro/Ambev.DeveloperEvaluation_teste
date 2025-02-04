using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;

/// <summary>
/// Validator for sale confirmation request
/// </summary>
public sealed class ConfirmSaleRequestValidator : AbstractValidator<ConfirmSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for sale confirmation
    /// </summary>
    public ConfirmSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 