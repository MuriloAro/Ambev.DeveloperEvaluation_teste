namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public sealed record GetBranchResult(
    Guid Id,
    string Name,
    string State,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
); 