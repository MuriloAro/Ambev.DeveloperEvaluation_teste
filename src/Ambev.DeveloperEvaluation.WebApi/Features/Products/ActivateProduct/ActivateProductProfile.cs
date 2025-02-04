using Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.ActivateProduct;

/// <summary>
/// AutoMapper profile for product activation mappings
/// </summary>
public sealed class ActivateProductProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product activation
    /// </summary>
    public ActivateProductProfile()
    {
        CreateMap<ActivateProductRequest, ActivateProductCommand>();
    }
} 