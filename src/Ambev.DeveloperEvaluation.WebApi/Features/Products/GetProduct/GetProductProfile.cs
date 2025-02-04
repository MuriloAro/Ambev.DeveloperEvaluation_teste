using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

/// <summary>
/// AutoMapper profile for product retrieval mappings
/// </summary>
public sealed class GetProductProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product retrieval
    /// </summary>
    public GetProductProfile()
    {
        CreateMap<GetProductRequest, GetProductQuery>();
        CreateMap<ProductDto, GetProductResponse>();
    }
} 