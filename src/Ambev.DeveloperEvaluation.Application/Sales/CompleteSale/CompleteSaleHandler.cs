using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;

/// <summary>
/// Handler for processing sale completion commands
/// </summary>
public sealed class CompleteSaleHandler : IRequestHandler<CompleteSaleCommand, CompleteSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CompleteSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the CompleteSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    public CompleteSaleHandler(ISaleRepository saleRepository, ILogger<CompleteSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the sale completion command
    /// </summary>
    /// <param name="request">The completion command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the completion operation</returns>
    public async Task<CompleteSaleResult> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Completing sale with ID: {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id);
        if (sale == null)
        {
            throw new DomainException("Sale not found");
        }

        sale.Complete();
        await _saleRepository.UpdateAsync(sale);

        _logger.LogInformation("Sale completed successfully");

        return new CompleteSaleResult
        {
            Success = true,
            Message = "Sale completed successfully"
        };
    }
} 