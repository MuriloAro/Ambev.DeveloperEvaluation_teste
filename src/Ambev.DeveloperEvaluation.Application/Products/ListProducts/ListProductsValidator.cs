using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Validator for product listing command
/// </summary>
public sealed class ListProductsValidator : AbstractValidator<ListProductsCommand>
{
    /// <summary>
    /// Initializes validation rules for product listing
    /// </summary>
    public ListProductsValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");
    }
} 