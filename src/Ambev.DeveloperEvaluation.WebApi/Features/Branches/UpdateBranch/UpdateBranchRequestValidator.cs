using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Validator for branch update request
/// </summary>
public sealed class UpdateBranchRequestValidator : AbstractValidator<UpdateBranchRequest>
{
    /// <summary>
    /// Initializes validation rules for branch update
    /// </summary>
    public UpdateBranchRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Branch ID is required");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name is required")
            .MinimumLength(3)
            .WithMessage("Branch name must be at least 3 characters")
            .MaximumLength(100)
            .WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State is required")
            .Length(2)
            .WithMessage("State must be exactly 2 characters");
    }
} 