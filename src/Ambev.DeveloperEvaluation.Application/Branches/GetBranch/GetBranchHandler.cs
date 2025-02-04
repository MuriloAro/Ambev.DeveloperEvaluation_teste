using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// Handler for processing branch retrieval commands
/// </summary>
public sealed class GetBranchHandler : IRequestHandler<GetBranchCommand, GetBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBranchHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the GetBranchHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetBranchHandler(IBranchRepository branchRepository, IMapper mapper, ILogger<GetBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the branch retrieval command
    /// </summary>
    /// <param name="request">The retrieval command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the retrieval operation</returns>
    public async Task<GetBranchResult> Handle(GetBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting branch with ID: {BranchId}", request.Id);

        var branch = await _branchRepository.GetByIdAsync(request.Id);
        
        if (branch == null)
            throw new DomainException("Branch not found");

        return _mapper.Map<GetBranchResult>(branch);
    }
} 