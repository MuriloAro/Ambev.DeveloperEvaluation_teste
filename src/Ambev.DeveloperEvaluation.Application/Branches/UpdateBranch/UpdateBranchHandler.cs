using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

public sealed class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<UpdateBranchHandler> _logger;

    public UpdateBranchHandler(IBranchRepository branchRepository, ILogger<UpdateBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<UpdateBranchResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
            throw new DomainException("Branch not found");

        branch.Update(request.Name, request.State);
        branch.SetActive(request.IsActive);

        await _branchRepository.UpdateAsync(branch);

        _logger.LogInformation("Branch updated successfully");

        return new UpdateBranchResult(
            branch.Id,
            branch.Name,
            branch.State,
            branch.IsActive,
            branch.CreatedAt,
            branch.UpdatedAt
        );
    }
} 