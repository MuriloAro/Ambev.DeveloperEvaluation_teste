using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

public sealed class DeleteBranchHandler : IRequestHandler<DeleteBranchCommand>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<DeleteBranchHandler> _logger;

    public DeleteBranchHandler(IBranchRepository branchRepository, ILogger<DeleteBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task Handle(DeleteBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
            throw new DomainException("Branch not found");

        await _branchRepository.DeleteAsync(request.Id);

        _logger.LogInformation("Branch deleted successfully");
    }
} 