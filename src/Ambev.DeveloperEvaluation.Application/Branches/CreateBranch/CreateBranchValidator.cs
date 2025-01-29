using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public sealed class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Branch name cannot be empty")
            .MaximumLength(100)
            .WithMessage("Branch name cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty()
            .WithMessage("State cannot be empty")
            .Length(2)
            .WithMessage("State must be exactly 2 characters");
    }
} 