using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Domain.Events;

public class SaleModifiedEvent
{
    public Guid SaleId { get; }
    public string SaleNumber { get; }
    public SaleStatus PreviousStatus { get; }
    public SaleStatus CurrentStatus { get; }
    public DateTime ModifiedAt { get; }

    public SaleModifiedEvent(Guid saleId, string saleNumber, SaleStatus previousStatus, SaleStatus currentStatus)
    {
        SaleId = saleId;
        SaleNumber = saleNumber;
        PreviousStatus = previousStatus;
        CurrentStatus = currentStatus;
        ModifiedAt = DateTime.UtcNow;
    }
} 