using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

/// <summary>
/// Validator for branch deletion request
/// </summary>
public sealed class DeleteBranchRequestValidator : AbstractValidator<DeleteBranchRequest>
{
    /// <summary>
    /// Initializes validation rules for branch deletion
    /// </summary>
    public DeleteBranchRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");
    }
} 