using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeactivateProduct;

/// <summary>
/// Validator for product deactivation request
/// </summary>
public sealed class DeactivateProductRequestValidator : AbstractValidator<DeactivateProductRequest>
{
    /// <summary>
    /// Initializes validation rules for product deactivation
    /// </summary>
    public DeactivateProductRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
} 