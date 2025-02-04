using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Handler for processing branch update commands
/// </summary>
public sealed class UpdateBranchHandler : IRequestHandler<UpdateBranchCommand, UpdateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateBranchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the UpdateBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public UpdateBranchHandler(IBranchRepository branchRepository, IMapper mapper, ILogger<UpdateBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the branch update command
    /// </summary>
    /// <param name="request">The update command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the update operation</returns>
    public async Task<UpdateBranchResult> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id);
        if (branch == null)
        {
            throw new DomainException("Branch not found");
        }

        branch.Update(request.Name, request.State);
        await _branchRepository.UpdateAsync(branch);

        _logger.LogInformation("Branch updated successfully");

        return _mapper.Map<UpdateBranchResult>(branch);
    }
} 