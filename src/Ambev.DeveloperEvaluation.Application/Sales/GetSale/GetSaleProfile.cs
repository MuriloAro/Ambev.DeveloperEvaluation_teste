using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// AutoMapper profile for sale retrieval mappings
/// </summary>
public sealed class GetSaleProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for sale retrieval
    /// </summary>
    public GetSaleProfile()
    {
        CreateMap<Sale, GetSaleResult>();
        CreateMap<SaleItem, SaleItemResult>();
    }
} 