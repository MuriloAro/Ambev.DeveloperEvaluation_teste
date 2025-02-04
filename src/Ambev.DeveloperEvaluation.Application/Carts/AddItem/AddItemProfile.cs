using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.AddItem;

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
        CreateMap<AddItemCommand, AddItemResult>();
    }
} 