namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// Request model for updating product stock
/// </summary>
public sealed class UpdateStockRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the product to update stock
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the new stock quantity
    /// Must be greater than or equal to 0
    /// </summary>
    public int StockQuantity { get; set; }
} 