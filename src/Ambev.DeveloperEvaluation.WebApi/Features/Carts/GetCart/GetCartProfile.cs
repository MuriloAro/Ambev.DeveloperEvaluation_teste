using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.DTOs;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

/// <summary>
/// AutoMapper profile for cart retrieval mappings
/// </summary>
public sealed class GetCartProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart retrieval
    /// </summary>
    public GetCartProfile()
    {
        CreateMap<GetCartRequest, GetCartByIdQuery>();
        CreateMap<CartDto, GetCartResponse>();
    }
} 