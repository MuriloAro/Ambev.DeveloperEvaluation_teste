namespace Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

/// <summary>
/// Result model for branch deletion operation
/// </summary>
public sealed class DeleteBranchResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the deleted branch
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }
} 