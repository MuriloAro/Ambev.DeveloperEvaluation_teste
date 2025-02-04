namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Response model for branch retrieval
/// </summary>
public sealed class GetBranchResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch
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

    /// <summary>
    /// Gets or sets when the branch was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
} 