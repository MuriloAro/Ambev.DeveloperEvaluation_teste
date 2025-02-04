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
using Microsoft.AspNetCore.Authorization;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CompleteSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ConfirmSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller for managing sale operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the SalesController
    /// </summary>
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
    [Authorize(Roles = "Admin,Manager,Customer")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new GetSaleRequest { Id = id };
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

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
    /// Lists sales with pagination and filters
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
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
        var request = new ListSalesRequest
        {
            Page = page,
            PageSize = pageSize,
            Status = status,
            StartDate = startDate,
            EndDate = endDate
        };

        var validator = new ListSalesRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = _mapper.Map<ListSalesQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);
        var response = _mapper.Map<ListSalesResponse>(result);

        return Ok(new ApiResponseWithData<ListSalesResponse>
        {
            Success = true,
            Message = "Sales retrieved successfully",
            Data = response
        });
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
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponseWithData<ConfirmSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Confirm([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new ConfirmSaleRequest { Id = id };
            var validator = new ConfirmSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<ConfirmSaleCommand>(request);
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
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponseWithData<CompleteSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Complete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new CompleteSaleRequest { Id = id };
            var validator = new CompleteSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var command = _mapper.Map<CompleteSaleCommand>(request);
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
    [HttpPost("{id:guid}/cancel")]
    [Authorize(Roles = "Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponseWithData<CancelSaleResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Cancel([FromRoute] Guid id, [FromBody] string reason, CancellationToken cancellationToken)
    {
        try
        {
            var request = new CancelSaleRequest { Reason = reason };
            var validator = new CancelSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

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
} 