using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public sealed record ListBranchesQuery() : IRequest<IEnumerable<ListBranchesResult>>; 