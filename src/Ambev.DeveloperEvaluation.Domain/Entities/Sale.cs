using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    public string Number { get; private set; } = string.Empty;
    public DateTime Date { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid BranchId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public SaleStatus Status { get; private set; }
    private readonly List<SaleItem> _items = new();
    public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();
    private readonly List<object> _domainEvents = new();
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();
    
    protected Sale() { } // Para o EF Core

    public Sale(Guid customerId, Guid branchId)
    {
        CustomerId = customerId;
        BranchId = branchId;
        Date = DateTime.UtcNow;
        Status = SaleStatus.Pending;
        Number = GenerateNumber();
        _domainEvents.Add(new SaleCreatedEvent(Id, Number));
    }

    private string GenerateNumber()
    {
        return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString()[..8].ToUpper()}";
    }

    public ValidationResultDetail Validate()
    {
        var validator = new SaleValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }

    public void Confirm()
    {
        if (Status != SaleStatus.Pending)
            throw new DomainException("Only pending sales can be confirmed");

        if (!_items.Any())
            throw new DomainException("Cannot confirm a sale without items");

        Status = SaleStatus.Confirmed;
        // Opcional: Publicar evento SaleConfirmed
    }

    public void Complete()
    {
        if (Status != SaleStatus.Confirmed)
            throw new DomainException("Only confirmed sales can be completed");

        Status = SaleStatus.Completed;
        // Opcional: Publicar evento SaleCompleted
    }

    public void Cancel(string reason)
    {
        if (Status == SaleStatus.Completed)
            throw new DomainException("Completed sales cannot be cancelled");

        if (Status == SaleStatus.Cancelled)
            throw new DomainException("Sale is already cancelled");

        var previousStatus = Status;
        Status = SaleStatus.Cancelled;

        _domainEvents.Add(new SaleCancelledEvent(Id, Number, reason));
        _domainEvents.Add(new SaleModifiedEvent(Id, Number, previousStatus, Status));
    }

    public void AddItem(Guid productId, int quantity, decimal unitPrice)
    {
        if (Status != SaleStatus.Pending)
            throw new DomainException("Can only add items to pending sales");

        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items");

        var previousStatus = Status;
        var item = new SaleItem(productId, quantity, unitPrice);
        _items.Add(item);
        RecalculateTotalAmount();

        _domainEvents.Add(new SaleModifiedEvent(Id, Number, previousStatus, Status));
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = _items.Sum(item => item.TotalAmount);
    }
} 