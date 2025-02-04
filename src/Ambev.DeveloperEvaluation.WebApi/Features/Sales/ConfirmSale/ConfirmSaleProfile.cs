using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;

/// <summary>
/// AutoMapper profile for sale confirmation mappings
/// </summary>
public sealed class ConfirmSaleProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for sale confirmation
    /// </summary>
    public ConfirmSaleProfile()
    {
        CreateMap<ConfirmSaleRequest, ConfirmSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
} 