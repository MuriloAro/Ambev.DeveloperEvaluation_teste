using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Carts.Checkout;

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
        CreateMap<Sale, CheckoutResult>();
    }
} 