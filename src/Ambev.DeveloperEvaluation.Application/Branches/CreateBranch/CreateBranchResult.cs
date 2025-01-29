namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public sealed record CreateBranchResult(
    Guid Id,
    string Name,
    string State,
    bool IsActive,
    DateTime CreatedAt
); 