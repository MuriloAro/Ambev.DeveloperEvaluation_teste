using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public sealed record GetBranchQuery(Guid Id) : IRequest<GetBranchResult>; 