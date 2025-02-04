using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;

/// <summary>
/// AutoMapper profile for sale cancellation mappings
/// </summary>
public sealed class CancelSaleProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for sale cancellation
    /// </summary>
    public CancelSaleProfile()
    {
        CreateMap<(Guid Id, string Reason), CancelSaleCommand>()
            .ConstructUsing(src => new CancelSaleCommand(src.Id, src.Reason));
    }
} 