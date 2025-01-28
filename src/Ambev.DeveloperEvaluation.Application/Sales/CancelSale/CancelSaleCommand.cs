using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

public record CancelSaleCommand(Guid SaleId, string Reason) : IRequest<CancelSaleResult>;

public class CancelSaleResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
} 