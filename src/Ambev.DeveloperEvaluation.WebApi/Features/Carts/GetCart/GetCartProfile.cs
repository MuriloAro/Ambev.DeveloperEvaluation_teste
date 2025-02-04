using Ambev.DeveloperEvaluation.Application.Carts.GetById;
using AutoMapper;

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
        CreateMap<GetCartRequest, GetByIdCommand>();
        CreateMap<GetByIdResult, GetCartResponse>();
    }
} 