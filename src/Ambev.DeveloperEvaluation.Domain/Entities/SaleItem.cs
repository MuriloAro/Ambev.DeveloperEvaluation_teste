using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool Cancelled { get; private set; }

    public SaleItem(Guid productId, int quantity, decimal unitPrice)
    {
        ProductId = productId;
        UnitPrice = unitPrice;
        UpdateQuantity(quantity);
    }

    public void UpdateQuantity(int quantity)
    {
        Quantity = quantity;
        CalculateDiscount();
        CalculateTotalAmount();
    }

    private void CalculateDiscount()
    {
        Discount = Quantity switch
        {
            >= 10 and <= 20 => 0.20m, // 20% discount
            >= 4 and < 10 => 0.10m,   // 10% discount
            _ => 0                     // no discount
        };
    }

    private void CalculateTotalAmount()
    {
        var subtotal = Quantity * UnitPrice;
        TotalAmount = subtotal - (subtotal * Discount);
    }

    public void Cancel()
    {
        if (Cancelled)
            throw new DomainException("Item is already cancelled");

        Cancelled = true;
        // Opcional: Publicar evento ItemCancelled
    }
} 