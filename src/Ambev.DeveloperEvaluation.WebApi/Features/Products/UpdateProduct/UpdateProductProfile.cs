using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;

/// <summary>
/// AutoMapper profile for product update mappings
/// </summary>
public sealed class UpdateProductProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product update
    /// </summary>
    public UpdateProductProfile()
    {
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<ProductDto, UpdateProductResponse>();
    }
} 