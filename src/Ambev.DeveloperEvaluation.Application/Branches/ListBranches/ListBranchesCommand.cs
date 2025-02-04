using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// Command for listing branches with pagination
/// </summary>
public sealed class ListBranchesCommand : IRequest<ListBranchesResult>
{
    /// <summary>
    /// Gets or sets the page number (starts at 1)
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size (max 100)
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets whether to include only active branches
    /// </summary>
    public bool? OnlyActive { get; set; }
} 