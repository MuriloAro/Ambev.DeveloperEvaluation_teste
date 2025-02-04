using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

/// <summary>
/// AutoMapper profile for product listing mappings
/// </summary>
public sealed class ListProductsProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product listing
    /// </summary>
    public ListProductsProfile()
    {
        CreateMap<ListProductsRequest, ListProductsCommand>();
        CreateMap<ListProductsResult, ListProductsResponse>()
            .ForMember(dest => dest.CurrentPage, opt => opt.MapFrom(src => src.CurrentPage))
            .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => 
                (int)Math.Ceiling((double)src.TotalCount / src.PageSize)));
    }
} 