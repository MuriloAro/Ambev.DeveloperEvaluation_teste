using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Command for deleting a branch
/// </summary>
public sealed class DeleteBranchCommand : IRequest<DeleteBranchResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to delete
    /// </summary>
    public Guid Id { get; set; }
} 