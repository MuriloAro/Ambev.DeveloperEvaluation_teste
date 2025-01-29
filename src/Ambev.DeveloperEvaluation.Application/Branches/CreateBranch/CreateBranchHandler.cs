using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

public sealed class CreateBranchHandler : IRequestHandler<CreateBranchCommand, CreateBranchResult>
{
    private readonly IBranchRepository _branchRepository;
    private readonly ILogger<CreateBranchHandler> _logger;

    public CreateBranchHandler(IBranchRepository branchRepository, ILogger<CreateBranchHandler> logger)
    {
        _branchRepository = branchRepository;
        _logger = logger;
    }

    public async Task<CreateBranchResult> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new branch with name: {Name}", request.Name);

        var branch = new Domain.Entities.Branch(request.Name, request.State);
        
        await _branchRepository.AddAsync(branch);

        _logger.LogInformation("Branch created successfully with ID: {Id}", branch.Id);

        return new CreateBranchResult(
            branch.Id,
            branch.Name,
            branch.State,
            branch.IsActive,
            branch.CreatedAt
        );
    }
} 