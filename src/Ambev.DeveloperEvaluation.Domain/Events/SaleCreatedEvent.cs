namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCreatedEvent
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }
    public DateTime CreatedAt { get; }

    public SaleCreatedEvent(Guid saleId, string saleNumber)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        CreatedAt = DateTime.UtcNow;
    }
} 