using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetById;

/// <summary>
/// AutoMapper profile for get cart by ID mappings
/// </summary>
public sealed class GetByIdProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for get cart by ID
    /// </summary>
    public GetByIdProfile()
    {
        CreateMap<Cart, GetByIdResult>();
        CreateMap<CartItem, CartItemResult>();
    }
} 