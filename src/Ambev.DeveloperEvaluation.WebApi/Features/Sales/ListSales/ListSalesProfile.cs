using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

/// <summary>
/// AutoMapper profile for sales listing mappings
/// </summary>
public sealed class ListSalesProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for sales listing
    /// </summary>
    public ListSalesProfile()
    {
        CreateMap<ListSalesRequest, ListSalesCommand>();
        CreateMap<ListSalesResult, ListSalesResponse>();
        CreateMap<SaleItemResult, SaleItemResponse>();
    }
} 