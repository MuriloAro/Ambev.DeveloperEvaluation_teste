using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ActivateProduct;

/// <summary>
/// Validator for product activation request
/// </summary>
public sealed class ActivateProductRequestValidator : AbstractValidator<ActivateProductRequest>
{
    /// <summary>
    /// Initializes validation rules for product activation
    /// </summary>
    public ActivateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
} 