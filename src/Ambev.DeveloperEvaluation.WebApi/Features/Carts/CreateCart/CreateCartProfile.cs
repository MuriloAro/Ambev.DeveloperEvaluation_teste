using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.DTOs;

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
        CreateMap<CreateCartRequest, CreateCartCommand>();
        CreateMap<CartDto, CreateCartResponse>();
    }
} 