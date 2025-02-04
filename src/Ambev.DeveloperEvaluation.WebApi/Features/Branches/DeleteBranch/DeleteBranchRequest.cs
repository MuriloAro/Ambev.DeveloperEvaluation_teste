namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

/// <summary>
/// Request model for deleting a branch
/// </summary>
public sealed class DeleteBranchRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to delete
    /// </summary>
    public Guid Id { get; set; }
} 