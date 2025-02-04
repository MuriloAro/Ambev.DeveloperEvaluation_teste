using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale;

/// <summary>
/// Handler for processing sale retrieval commands
/// </summary>
public sealed class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the GetSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="mapper">The automapper instance</param>
    /// <param name="logger">The logger instance</param>
    public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper, ILogger<GetSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    /// Handles the sale retrieval command
    /// </summary>
    /// <param name="request">The retrieval command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the retrieval operation</returns>
    public async Task<GetSaleResult> Handle(GetSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting sale with ID: {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id);
        if (sale == null)
        {
            throw new DomainException("Sale not found");
        }

        var result = _mapper.Map<GetSaleResult>(sale);

        _logger.LogInformation("Sale retrieved successfully");

        return result;
    }
} 