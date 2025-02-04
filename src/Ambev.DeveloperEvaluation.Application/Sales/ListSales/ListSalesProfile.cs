using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSales;

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
        CreateMap<Sale, SaleItemResult>();
    }
} 