using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

/// <summary>
/// Validator for product activation command
/// </summary>
public sealed class ActivateProductValidator : AbstractValidator<ActivateProductCommand>
{
    /// <summary>
    /// Initializes validation rules for product activation
    /// </summary>
    public ActivateProductValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Product ID is required");
    }
} 