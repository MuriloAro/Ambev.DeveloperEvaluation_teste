namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public sealed record ListBranchesResult(
    Guid Id,
    string Name,
    string State,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
); 