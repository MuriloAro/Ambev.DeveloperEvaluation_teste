using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public sealed record CreateBranchCommand(
    string Name,
    string State
) : IRequest<CreateBranchResult>; 