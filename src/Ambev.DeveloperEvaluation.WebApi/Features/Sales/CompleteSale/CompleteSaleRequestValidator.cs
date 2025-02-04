using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CompleteSale;

/// <summary>
/// Validator for sale completion request
/// </summary>
public sealed class CompleteSaleRequestValidator : AbstractValidator<CompleteSaleRequest>
{
    /// <summary>
    /// Initializes validation rules for sale completion
    /// </summary>
    public CompleteSaleRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Sale ID is required");
    }
} 