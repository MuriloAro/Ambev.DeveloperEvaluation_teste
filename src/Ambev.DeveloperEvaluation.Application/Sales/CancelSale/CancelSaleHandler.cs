using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

/// <summary>
/// Handler for processing sale cancellation commands
/// </summary>
public sealed class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CancelSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the CancelSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    public CancelSaleHandler(ISaleRepository saleRepository, ILogger<CancelSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the sale cancellation command
    /// </summary>
    /// <param name="request">The cancellation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the cancellation operation</returns>
    public async Task<CancelSaleResult> Handle(CancelSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Canceling sale with ID: {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id);
        if (sale == null)
        {
            throw new DomainException("Sale not found");
        }

        sale.Cancel(request.Reason);
        await _saleRepository.UpdateAsync(sale);

        _logger.LogInformation("Sale canceled successfully");

        return new CancelSaleResult
        {
            Success = true,
            Message = "Sale canceled successfully"
        };
    }
} 