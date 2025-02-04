using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProduct;

/// <summary>
/// Handler for processing product retrieval commands
/// </summary>
public sealed class GetProductHandler : IRequestHandler<GetProductCommand, GetProductResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the GetProductHandler
    /// </summary>
    /// <param name="productRepository">The product repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetProductHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the product retrieval command
    /// </summary>
    /// <param name="request">The retrieval command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the retrieval operation</returns>
    public async Task<GetProductResult> Handle(GetProductCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting product with ID: {ProductId}", request.Id);

        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new DomainException("Product not found");
        }

        return _mapper.Map<GetProductResult>(product);
    }
} 