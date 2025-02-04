namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// Response model for product stock update
/// </summary>
public sealed class UpdateStockResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the product name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated stock quantity
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Gets or sets when the stock was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
} 