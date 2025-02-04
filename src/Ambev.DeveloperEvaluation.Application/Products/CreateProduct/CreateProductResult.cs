namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

/// <summary>
/// Result model for product creation operation
/// </summary>
public sealed class CreateProductResult
{
    /// <summary>
    /// Gets or sets whether the operation was successful
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the message describing the result
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the ID of the created product
    /// </summary>
    public Guid ProductId { get; set; }
} 