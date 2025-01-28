namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleCancelledEvent
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }
    public string Reason { get; }
    public DateTime CancelledAt { get; }

    public SaleCancelledEvent(Guid saleId, string saleNumber, string reason)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        Reason = reason;
        CancelledAt = DateTime.UtcNow;
    }
} 