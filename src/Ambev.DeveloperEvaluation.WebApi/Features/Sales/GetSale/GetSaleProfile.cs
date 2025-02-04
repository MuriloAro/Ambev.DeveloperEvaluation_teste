using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

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
        CreateMap<GetSaleRequest, GetSaleCommand>();
        CreateMap<GetSaleResult, GetSaleResponse>();
        CreateMap<SaleItemResult, GetSaleItemResponse>();
    }
} 