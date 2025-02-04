using MediatR;
using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

public class ListSalesQuery : IRequest<ListSalesResult>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SaleStatus? Status { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class ListSalesResult
{
    public IEnumerable<SaleDto> Items { get; set; } = new List<SaleDto>();
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
}

//public class SaleDto
//{
//    public Guid Id { get; set; }
//    public string Number { get; set; } = string.Empty;
//    public DateTime Date { get; set; }
//    public Guid CustomerId { get; set; }
//    public Guid BranchId { get; set; }
//    public decimal TotalAmount { get; set; }
//    public SaleStatus Status { get; set; }
//} 