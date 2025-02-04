using Ambev.DeveloperEvaluation.Application.Carts.AddItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItem;

/// <summary>
/// AutoMapper profile for cart item addition mappings
/// </summary>
public sealed class AddItemProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart item addition
    /// </summary>
    public AddItemProfile()
    {
        CreateMap<AddItemRequest, AddItemCommand>();
        CreateMap<CartItemDto, AddItemResponse>();
    }
} 