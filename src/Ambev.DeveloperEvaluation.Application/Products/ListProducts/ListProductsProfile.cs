using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

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
        CreateMap<Product, ProductItemResult>();
    }
} 