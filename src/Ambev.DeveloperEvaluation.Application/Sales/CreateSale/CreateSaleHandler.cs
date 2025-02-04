using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

/// <summary>
/// Handler for processing sale creation commands
/// </summary>
public sealed class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ILogger<CreateSaleHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the CreateSaleHandler
    /// </summary>
    /// <param name="saleRepository">The sale repository</param>
    /// <param name="logger">The logger instance</param>
    public CreateSaleHandler(ISaleRepository saleRepository, ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _logger = logger;
    }

    /// <summary>
    /// Handles the sale creation command
    /// </summary>
    /// <param name="request">The creation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the creation operation</returns>
    public async Task<CreateSaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new sale for customer: {CustomerId} at branch: {BranchId}", 
            request.CustomerId, request.BranchId);

        var sale = new Sale(request.CustomerId, request.BranchId);

        foreach (var item in request.Items)
        {
            sale.AddItem(item.ProductId, item.Quantity, item.UnitPrice);
        }

        await _saleRepository.CreateAsync(sale);

        _logger.LogInformation("Sale created successfully with ID: {SaleId}", sale.Id);

        return new CreateSaleResult
        {
            Success = true,
            Message = "Sale created successfully",
            SaleId = sale.Id
        };
    }
} 