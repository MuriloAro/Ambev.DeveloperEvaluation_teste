using Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.DeactivateProduct;

/// <summary>
/// AutoMapper profile for product deactivation mappings
/// </summary>
public sealed class DeactivateProductProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for product deactivation
    /// </summary>
    public DeactivateProductProfile()
    {
        CreateMap<DeactivateProductRequest, DeactivateProductCommand>();
    }
} 