namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;

/// <summary>
/// Request model for confirming a sale
/// </summary>
public sealed class ConfirmSaleRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the sale to confirm
    /// </summary>
    public Guid Id { get; set; }
} 