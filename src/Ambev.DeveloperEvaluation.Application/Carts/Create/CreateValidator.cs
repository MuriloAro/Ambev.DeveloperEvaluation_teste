using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.Create;

/// <summary>
/// Validator for cart creation command
/// </summary>
public sealed class CreateValidator : AbstractValidator<CreateCommand>
{
    /// <summary>
    /// Initializes validation rules for cart creation
    /// </summary>
    public CreateValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required");
    }
} 