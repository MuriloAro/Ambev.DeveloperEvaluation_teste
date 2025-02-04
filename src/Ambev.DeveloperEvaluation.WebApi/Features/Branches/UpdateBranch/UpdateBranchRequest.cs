namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

/// <summary>
/// Request model for updating a branch
/// </summary>
public sealed class UpdateBranchRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to update
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the branch name
    /// Must be between 3 and 100 characters
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch state
    /// Must be exactly 2 characters (e.g., SP, RJ)
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the branch is active
    /// </summary>
    public bool IsActive { get; set; }
} 