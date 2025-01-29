using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public sealed record DeleteBranchCommand(Guid Id) : IRequest; 