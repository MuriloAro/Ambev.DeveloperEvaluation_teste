using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ListSalesResult, ListSalesResponse>();
        CreateMap<SaleDto, SaleItemResponse>();
        CreateMap<CreateProductRequest, CreateProductCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<GetProductResult, GetProductResponse>();
        CreateMap<ListProductsResult, ListProductsResponse>();
        CreateMap<ProductDto, ProductItemResponse>();
    }
} 