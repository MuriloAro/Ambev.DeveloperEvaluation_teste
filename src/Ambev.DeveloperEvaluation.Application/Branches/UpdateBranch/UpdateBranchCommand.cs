using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

public sealed record UpdateBranchCommand(
    Guid Id,
    string Name,
    string State,
    bool IsActive
) : IRequest<UpdateBranchResult>; 