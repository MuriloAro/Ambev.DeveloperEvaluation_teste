using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Products.ListProducts;

/// <summary>
/// Handler for processing product listing commands
/// </summary>
public sealed class ListProductsHandler : IRequestHandler<ListProductsCommand, ListProductsResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ListProductsHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the ListProductsHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public ListProductsHandler(IProductRepository productRepository, IMapper mapper, ILogger<ListProductsHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the product listing command
    /// </summary>
    /// <param name="request">The listing command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the listing operation</returns>
    public async Task<ListProductsResult> Handle(ListProductsCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Listing products. Page: {Page}, PageSize: {PageSize}", request.Page, request.PageSize);

        var (products, totalCount) = await _productRepository.ListAsync(
            page: request.Page,
            pageSize: request.PageSize,
            category: request.Category
        );

        var result = new ListProductsResult
        {
            Items = _mapper.Map<ICollection<ProductItemResult>>(products),
            TotalCount = totalCount,
            CurrentPage = request.Page,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };

        _logger.LogInformation("Found {Count} products", totalCount);

        return result;
    }
} 