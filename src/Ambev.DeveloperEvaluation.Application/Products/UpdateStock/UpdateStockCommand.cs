using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

/// <summary>
/// Command for updating a product's stock quantity
/// </summary>
public sealed class UpdateStockCommand : IRequest<UpdateStockResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Gets or sets the new stock quantity
    /// </summary>
    public int Quantity { get; set; }
}