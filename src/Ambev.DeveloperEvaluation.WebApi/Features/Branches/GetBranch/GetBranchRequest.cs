namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

/// <summary>
/// Request model for retrieving a branch
/// </summary>
public sealed class GetBranchRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the branch to retrieve
    /// </summary>
    public Guid Id { get; set; }
} 