using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.Create;

/// <summary>
/// AutoMapper profile for cart creation mappings
/// </summary>
public sealed class CreateProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart creation
    /// </summary>
    public CreateProfile()
    {
        CreateMap<Cart, CreateResult>();
    }
} 