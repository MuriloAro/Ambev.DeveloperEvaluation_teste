using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;

/// <summary>
/// AutoMapper profile for product creation mappings
/// </summary>
public sealed class CreateProductProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product creation
    /// </summary>
    public CreateProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<ProductDto, CreateProductResponse>();
    }
} 