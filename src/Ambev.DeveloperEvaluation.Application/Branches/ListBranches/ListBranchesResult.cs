namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Result model for branch listing operation
/// </summary>
public sealed class ListBranchesResult
{
    /// <summary>
    /// Gets or sets the list of branches
    /// </summary>
    public ICollection<BranchItemResult> Items { get; set; } = new List<BranchItemResult>();

    /// <summary>
    /// Gets or sets the total number of branches
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the current page number
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets the page size
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages
    /// </summary>
    public int TotalPages { get; set; }
}

/// <summary>
/// Result model for branch item in list
/// </summary>
public sealed class BranchItemResult
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch
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