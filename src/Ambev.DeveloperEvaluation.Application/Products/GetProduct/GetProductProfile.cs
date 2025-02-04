using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

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
        CreateMap<Product, GetProductResult>();
    }
} 