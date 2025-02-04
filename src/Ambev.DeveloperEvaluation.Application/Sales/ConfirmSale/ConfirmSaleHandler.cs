using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;

/// <summary>
/// Handler for processing sale confirmation commands
/// </summary>
public sealed class ConfirmSaleHandler : IRequestHandler<ConfirmSaleCommand, ConfirmSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<ConfirmSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the ConfirmSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    public ConfirmSaleHandler(ISaleRepository saleRepository, ILogger<ConfirmSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the sale confirmation command
    /// </summary>
    /// <param name="request">The confirmation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the confirmation operation</returns>
    public async Task<ConfirmSaleResult> Handle(ConfirmSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Confirming sale with ID: {SaleId}", request.Id);

        var sale = await _saleRepository.GetByIdAsync(request.Id);
        if (sale == null)
        {
            throw new DomainException("Sale not found");
        }

        sale.Confirm();
        await _saleRepository.UpdateAsync(sale);

        _logger.LogInformation("Sale confirmed successfully");

        return new ConfirmSaleResult
        {
            Success = true,
            Message = "Sale confirmed successfully"
        };
    }
} 