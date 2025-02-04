namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

/// <summary>
/// Request model for listing branches
/// </summary>
public sealed class ListBranchesRequest
{
    /// <summary>
    /// Gets or sets the page number for pagination
    /// Must be greater than 0
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size for pagination
    /// Must be between 1 and 100
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets whether to include only active branches
    /// </summary>
    public bool? OnlyActive { get; set; }

    /// <summary>
    /// Gets or sets the state to filter branches
    /// </summary>
    public string? State { get; set; }
} 