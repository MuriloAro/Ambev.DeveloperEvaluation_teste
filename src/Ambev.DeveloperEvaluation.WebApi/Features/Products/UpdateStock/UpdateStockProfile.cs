using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

/// <summary>
/// AutoMapper profile for product stock update mappings
/// </summary>
public sealed class UpdateStockProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product stock update
    /// </summary>
    public UpdateStockProfile()
    {
        CreateMap<UpdateStockRequest, UpdateProductCommand>();
        CreateMap<ProductDto, UpdateStockResponse>();
    }
} 