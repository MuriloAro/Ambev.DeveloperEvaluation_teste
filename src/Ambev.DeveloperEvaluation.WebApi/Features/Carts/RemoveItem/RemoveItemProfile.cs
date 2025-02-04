using Ambev.DeveloperEvaluation.Application.Carts.RemoveItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.RemoveItem;

/// <summary>
/// AutoMapper profile for cart item removal mappings
/// </summary>
public sealed class RemoveItemProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart item removal
    /// </summary>
    public RemoveItemProfile()
    {
        CreateMap<RemoveItemRequest, RemoveItemCommand>();
    }
} 