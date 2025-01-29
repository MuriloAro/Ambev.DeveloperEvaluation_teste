using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItem;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItem;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.Checkout;
using Ambev.DeveloperEvaluation.Application.DTOs;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetActiveCartByUserId;
using Ambev.DeveloperEvaluation.Application.Sales.ListSales;
using Ambev.DeveloperEvaluation.WebApi.Models;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CartsController> _logger;

    public CartsController(IMediator mediator, ILogger<CartsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<CartDto>> Create([FromBody] CreateCartCommand command)
    {
        _logger.LogInformation("Creating cart for user {UserId}", command.UserId);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartDto>> GetById(Guid id)
    {
        _logger.LogInformation("Getting cart {CartId}", id);
        var result = await _mediator.Send(new GetCartByIdQuery { Id = id });
        return Ok(result);
    }

    [HttpGet("user/{userId}/active")]
    public async Task<ActionResult<CartDto>> GetActiveByUserId(Guid userId)
    {
        _logger.LogInformation("Getting active cart for user {UserId}", userId);
        var result = await _mediator.Send(new GetActiveCartByUserIdQuery { UserId = userId });
        
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpPost("{cartId}/items")]
    public async Task<ActionResult<CartDto>> AddItem(Guid cartId, [FromBody] AddCartItemRequest request)
    {
        _logger.LogInformation("Adding item to cart {CartId}", cartId);
        
        var command = new AddCartItemCommand
        {
            CartId = cartId,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{cartId}/items/{productId}")]
    public async Task<ActionResult<CartDto>> RemoveItem(Guid cartId, Guid productId)
    {
        _logger.LogInformation("Removing item from cart {CartId}", cartId);
        
        var command = new RemoveCartItemCommand
        {
            CartId = cartId,
            ProductId = productId
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("{cartId}/checkout")]
    public async Task<ActionResult<SaleDto>> Checkout(Guid cartId, [FromBody] CheckoutCartRequest request)
    {
        _logger.LogInformation("Processing checkout for cart {CartId}", cartId);
        
        var command = new CheckoutCartCommand
        {
            CartId = cartId,
            BranchId = request.BranchId
        };

        var result = await _mediator.Send(command);
        return Ok(result);
    }
} 