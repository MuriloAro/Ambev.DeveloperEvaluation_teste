using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Command for confirming a sale
/// </summary>
public sealed class ConfirmSaleCommand : IRequest<ConfirmSaleResult>
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to confirm
    /// </summary>
    public Guid Id { get; set; }
}