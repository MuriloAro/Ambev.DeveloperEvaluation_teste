namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleResult
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
} 