using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItem;
using Ambev.DeveloperEvaluation.Application.DTOs;

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
        CreateMap<AddItemRequest, AddCartItemCommand>();
        CreateMap<CartItemDto, AddItemResponse>();
    }
} 