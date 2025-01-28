using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Sales.ConfirmSale;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Application.Sales.CompleteSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SalesController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Retrieves a specific sale by ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The sale details if found</returns>
    /// <response code="200">Returns the sale details</response>
    /// <response code="404">If the sale is not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetSaleQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetSaleResponse>(result);

            return Ok(new ApiResponseWithData<GetSaleResponse>
            {
                Success = true,
                Message = "Sale retrieved successfully",
                Data = response
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = $"Sale with ID {id} not found"
            });
        }
    }

    /// <summary>
    /// Confirms a specific sale by ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the confirmation</returns>
    /// <response code="200">Returns the result of the confirmation</response>
    /// <response code="404">If the sale is not found</response>
    /// <response code="400">If the sale is invalid</response>
    [HttpPost("{id:guid}/confirm")]
    [ProducesResponseType(typeof(ApiResponseWithData<ConfirmSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new ConfirmSaleCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<ConfirmSaleResult>
            {
                Success = true,
                Message = "Sale confirmed successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = $"Sale with ID {id} not found"
            });
        }
        catch (DomainException ex)
        {
            return BadRequest(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Completes a specific sale by ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the completion</returns>
    /// <response code="200">Returns the result of the completion</response>
    /// <response code="404">If the sale is not found</response>
    /// <response code="400">If the sale cannot be completed</response>
    [HttpPost("{id:guid}/complete")]
    [ProducesResponseType(typeof(ApiResponseWithData<CompleteSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CompleteSaleCommand(id);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<CompleteSaleResult>
            {
                Success = true,
                Message = "Sale completed successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = $"Sale with ID {id} not found"
            });
        }
        catch (DomainException ex)
        {
            return BadRequest(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Cancels a specific sale by ID
    /// </summary>
    /// <param name="id">The unique identifier of the sale</param>
    /// <param name="reason">The reason for cancellation</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The result of the cancellation</returns>
    /// <response code="200">Returns the result of the cancellation</response>
    /// <response code="404">If the sale is not found</response>
    /// <response code="400">If the sale cannot be cancelled</response>
    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] string reason, CancellationToken cancellationToken)
    {
        try
        {
            var command = new CancelSaleCommand(id, reason);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<CancelSaleResult>
            {
                Success = true,
                Message = "Sale cancelled successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = $"Sale with ID {id} not found"
            });
        }
        catch (DomainException ex)
        {
            return BadRequest(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Lists sales with pagination and filters
    /// </summary>
    /// <param name="page">Page number (starts at 1)</param>
    /// <param name="pageSize">Items per page (max 100)</param>
    /// <param name="status">Filter by sale status</param>
    /// <param name="startDate">Filter by start date</param>
    /// <param name="endDate">Filter by end date</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paged list of sales</returns>
    /// <response code="200">Returns the paged list of sales</response>
    /// <response code="400">If the query parameters are invalid</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListSalesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] SaleStatus? status = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new ListSalesQuery
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                StartDate = startDate,
                EndDate = endDate
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<ListSalesResponse>(result);

            return Ok(new ApiResponseWithData<ListSalesResponse>
            {
                Success = true,
                Message = "Sales retrieved successfully",
                Data = response
            });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
            });
        }
    }
} 