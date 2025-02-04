using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateItem;

/// <summary>
/// AutoMapper profile for cart item update mappings
/// </summary>
public sealed class UpdateItemProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart item update
    /// </summary>
    public UpdateItemProfile()
    {
        CreateMap<CartItem, UpdateItemResult>()
            .ForMember(dest => dest.CartId, opt => opt.MapFrom(src => src.CartId));
    }
} 