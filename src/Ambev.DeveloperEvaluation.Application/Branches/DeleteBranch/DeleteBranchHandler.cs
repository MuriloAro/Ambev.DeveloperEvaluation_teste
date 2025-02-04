using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Handler for processing branch deletion commands
/// </summary>
public sealed class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand, DeleteBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<DeleteBranchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the DeleteBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="logger">The logger</param>
    public DeleteBranchHandler(IBranchRepository branchRepository, ILogger<DeleteBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the branch deletion command
    /// </summary>
    /// <param name="request">The deletion command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the deletion operation</returns>
    public async Task<DeleteBranchResult> Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new DomainException("Branch not found");
        }

        await _branchRepository.DeleteAsync(branch.Id);

        _logger.LogInformation("Branch deleted successfully");

        return new DeleteBranchResult
        {
            Id = request.Id,
            Success = true
        };
    }
} 