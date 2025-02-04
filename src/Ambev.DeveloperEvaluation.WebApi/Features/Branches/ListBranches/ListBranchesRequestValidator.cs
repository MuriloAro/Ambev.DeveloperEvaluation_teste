using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Validator for branch listing request
/// </summary>
public sealed class ListBranchesRequestValidator : AbstractValidator<ListBranchesRequest>
{
    /// <summary>
    /// Initializes validation rules for branch listing
    /// </summary>
    public ListBranchesRequestValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100");

        When(x => !string.IsNullOrEmpty(x.State), () =>
        {
            RuleFor(x => x.State)
                .Length(2)
                .WithMessage("State must be exactly 2 characters");
        });
    }
} 