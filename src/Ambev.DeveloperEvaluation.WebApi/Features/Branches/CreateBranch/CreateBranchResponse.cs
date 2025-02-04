namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Response model for branch creation
/// </summary>
public sealed class CreateBranchResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the created branch
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the branch name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the branch state
    /// </summary>
    public string State { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the branch is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets when the branch was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
} 