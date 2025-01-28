using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

public record ConfirmSaleCommand(Guid SaleId) : IRequest<ConfirmSaleResult>;

public class ConfirmSaleResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
} 