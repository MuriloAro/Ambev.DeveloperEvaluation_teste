namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Result model for stock update operation
/// </summary>
public sealed class UpdateStockResult
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