namespace Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;

/// <summary>
/// Result model for product deactivation operation
/// </summary>
public sealed class DeactivateProductResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message describing the result
    /// </summary>
    public string Message { get; set; } = string.Empty;
} 