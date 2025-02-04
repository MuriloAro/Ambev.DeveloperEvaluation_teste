using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Command for canceling a sale
/// </summary>
public sealed class CancelSaleCommand : IRequest<CancelSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to cancel
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the reason for cancellation
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}