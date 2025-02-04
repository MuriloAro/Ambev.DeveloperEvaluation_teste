using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CompleteSale;

/// <summary>
/// AutoMapper profile for sale completion mappings
/// </summary>
public sealed class CompleteSaleProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for sale completion
    /// </summary>
    public CompleteSaleProfile()
    {
        CreateMap<CompleteSaleRequest, CompleteSaleCommand>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
} 