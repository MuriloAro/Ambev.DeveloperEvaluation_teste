using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Validator for branch retrieval request
/// </summary>
public sealed class GetBranchRequestValidator : AbstractValidator<GetBranchRequest>
{
    /// <summary>
    /// Initializes validation rules for branch retrieval
    /// </summary>
    public GetBranchRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
} 