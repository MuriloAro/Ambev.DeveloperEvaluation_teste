namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

/// <summary>
/// Request model for creating a new branch
/// </summary>
public sealed class CreateBranchRequest
{
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
} 