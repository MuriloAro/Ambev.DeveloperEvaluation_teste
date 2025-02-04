namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class SaleItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal Discount { get; set; }
    public bool Cancelled { get; set; }
} 