namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleItemCancelledEvent
{
    public Guid SaleId { get; }
    public Guid ItemId { get; }
    public Guid ProductId { get; }
    public int Quantity { get; }
    public DateTime CancelledAt { get; }

    public SaleItemCancelledEvent(Guid saleId, Guid itemId, Guid productId, int quantity)
    {
        SaleId = saleId;
        ItemId = itemId;
        ProductId = productId;
        Quantity = quantity;
        CancelledAt = DateTime.UtcNow;
    }
} 