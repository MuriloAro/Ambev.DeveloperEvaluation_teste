using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetActive;

/// <summary>
/// AutoMapper profile for get active cart mappings
/// </summary>
public sealed class GetActiveProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for get active cart
    /// </summary>
    public GetActiveProfile()
    {
        CreateMap<Cart, GetActiveResult>();
        CreateMap<CartItem, CartItemResult>();
    }
} 