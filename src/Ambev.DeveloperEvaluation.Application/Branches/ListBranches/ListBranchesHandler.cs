using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Handler for processing branch listing commands
/// </summary>
public sealed class ListBranchesHandler : IRequestHandler<ListBranchesCommand, ListBranchesResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListBranchesHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the ListBranchesHandler
    /// </summary>
    /// <param name="branchRepository">The branch repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public ListBranchesHandler(IBranchRepository branchRepository, IMapper mapper, ILogger<ListBranchesHandler> logger)
    {
        _branchRepository = branchRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the branch listing command
    /// </summary>
    /// <param name="request">The listing command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the listing operation</returns>
    public async Task<ListBranchesResult> Handle(ListBranchesCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing branches. Page: {Page}, PageSize: {PageSize}", request.Page, request.PageSize);

        var branches = await _branchRepository.GetAllAsync();
        var totalCount = branches.Count();

        var pagedBranches = branches
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new ListBranchesResult
        {
            Items = _mapper.Map<ICollection<BranchItemResult>>(pagedBranches),
            TotalCount = totalCount,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };

        _logger.LogInformation("Found {Count} branches", totalCount);

        return result;
    }
} 