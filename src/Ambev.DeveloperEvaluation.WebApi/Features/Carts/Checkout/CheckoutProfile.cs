using Ambev.DeveloperEvaluation.Application.Carts.Checkout;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.Checkout;

/// <summary>
/// AutoMapper profile for cart checkout mappings
/// </summary>
public sealed class CheckoutProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for cart checkout
    /// </summary>
    public CheckoutProfile()
    {
        CreateMap<CheckoutRequest, CheckoutCommand>();
        CreateMap<CheckoutResult, CheckoutResponse>();
    }
} 