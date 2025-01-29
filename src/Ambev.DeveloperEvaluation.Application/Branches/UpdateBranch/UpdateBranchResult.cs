namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

public sealed record UpdateBranchResult(
    Guid Id,
    string Name,
    string State,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
); 