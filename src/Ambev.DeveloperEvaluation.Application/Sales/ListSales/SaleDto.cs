using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class SaleDto
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public SaleStatus Status { get; set; }   
    public decimal TotalAmount { get; set; }
    public Guid CustomerId { get; set; }
    public Guid BranchId { get; set; }
    public ICollection<SaleItemDto> Items { get; set; } = new List<SaleItemDto>();
} 