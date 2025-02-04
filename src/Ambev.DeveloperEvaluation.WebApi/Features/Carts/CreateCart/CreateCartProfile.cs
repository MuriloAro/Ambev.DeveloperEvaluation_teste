using Ambev.DeveloperEvaluation.Application.Carts.Create;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;

/// <summary>
/// AutoMapper profile for cart creation mappings
/// </summary>
public sealed class CreateCartProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart creation
    /// </summary>
    public CreateCartProfile()
    {
        CreateMap<CreateCartRequest, CreateCommand>();
        CreateMap<CreateResult, CreateCartResponse>();
    }
} 