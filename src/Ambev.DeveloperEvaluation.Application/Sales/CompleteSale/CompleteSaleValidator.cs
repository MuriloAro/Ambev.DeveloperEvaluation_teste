using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

/// <summary>
/// Validator for sale completion command
/// </summary>
public sealed class CompleteSaleValidator : AbstractValidator<CompleteSaleCommand>
{
    /// <summary>
    /// Initializes validation rules for sale completion
    /// </summary>
    public CompleteSaleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 