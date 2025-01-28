using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.UpdateStock;

public class UpdateStockCommand : IRequest<UpdateStockResult>
{
    public Guid Id { get; set; }
    public int StockQuantity { get; set; }
}

public class UpdateStockResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
} 