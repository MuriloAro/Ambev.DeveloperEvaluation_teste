using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// Command for updating a branch
/// </summary>
public sealed class UpdateBranchCommand : IRequest<UpdateBranchResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the branch
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the state of the branch
    /// </summary>
    public string State { get; set; } = string.Empty;
} 