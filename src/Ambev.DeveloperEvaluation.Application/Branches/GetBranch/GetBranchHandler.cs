using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

public sealed class GetBranchHandler : IRequestHandler<GetBranchQuery, GetBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<GetBranchHandler> _logger;

    public GetBranchHandler(IBranchRepository branchRepository, ILogger<GetBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<GetBranchResult> Handle(GetBranchQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id);
        
        if (branch == null)
            throw new DomainException("Branch not found");

        return new GetBranchResult(
            branch.Id,
            branch.Name,
            branch.State,
            branch.IsActive,
            branch.CreatedAt,
            branch.UpdatedAt
        );
    }
} 