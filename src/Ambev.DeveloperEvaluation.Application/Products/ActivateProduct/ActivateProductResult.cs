namespace Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;

/// <summary>
/// Result model for product activation operation
/// </summary>
public sealed class ActivateProductResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the UpdatedAt
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Gets or sets the ID of the created product
    /// </summary>
    public Guid Id { get; set; }
}