using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// AutoMapper profile for sale cancellation mappings
/// </summary>
public sealed class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for sale cancellation
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<CancelSaleRequest, CancelSaleCommand>()
            .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason));
    }
} 