using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

public sealed class ListBranchesHandler : IRequestHandler<ListBranchesQuery, IEnumerable<ListBranchesResult>>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<ListBranchesHandler> _logger;

    public ListBranchesHandler(IBranchRepository branchRepository, ILogger<ListBranchesHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ListBranchesResult>> Handle(ListBranchesQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all branches");

        var branches = await _branchRepository.GetAllAsync();

        return branches.Select(branch => new ListBranchesResult(
            branch.Id,
            branch.Name,
            branch.State,
            branch.IsActive,
            branch.CreatedAt,
            branch.UpdatedAt
        ));
    }
} 