using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Validator for branch listing command
/// </summary>
public sealed class ListBranchesValidator : AbstractValidator<ListBranchesCommand>
{
    /// <summary>
    /// Initializes validation rules for branch listing
    /// </summary>
    public ListBranchesValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");
    }
} 