using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

public record CompleteSaleCommand(Guid SaleId) : IRequest<CompleteSaleResult>;

public class CompleteSaleResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
} 