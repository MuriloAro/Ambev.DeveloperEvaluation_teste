using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetById;

/// <summary>
/// Validator for retrieving cart by ID command
/// </summary>
public sealed class GetByIdValidator : AbstractValidator<GetByIdCommand>
{
    /// <summary>
    /// Initializes validation rules for retrieving cart by ID
    /// </summary>
    public GetByIdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Cart ID is required");
    }
} 