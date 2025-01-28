using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ListSalesResult, ListSalesResponse>();
        CreateMap<SaleDto, SaleItemResponse>();
    }
} 