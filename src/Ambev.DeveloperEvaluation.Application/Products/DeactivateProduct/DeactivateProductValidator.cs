using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

/// <summary>
/// Validator for product deactivation command
/// </summary>
public sealed class DeactivateProductValidator : AbstractValidator<DeactivateProductCommand>
{
    /// <summary>
    /// Initializes validation rules for product deactivation
    /// </summary>
    public DeactivateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
} 