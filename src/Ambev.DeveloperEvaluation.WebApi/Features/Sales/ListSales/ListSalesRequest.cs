using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// Request model for listing sales
/// </summary>
public sealed class ListSalesRequest
{
    /// <summary>
    /// Gets or sets the page number for pagination
    /// Must be greater than 0
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Gets or sets the page size for pagination
    /// Must be between 1 and 100
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the sale status filter
    /// </summary>
    public SaleStatus? Status { get; set; }

    /// <summary>
    /// Gets or sets the start date filter
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Gets or sets the end date filter
    /// </summary>
    public DateTime? EndDate { get; set; }
} 