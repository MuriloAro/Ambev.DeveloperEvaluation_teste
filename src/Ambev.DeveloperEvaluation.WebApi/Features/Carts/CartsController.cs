using Ambev.DeveloperEvaluation.Application.Commands.Carts.AddItem;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.Checkout;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Commands.Carts.RemoveItem;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetCartById;
using Ambev.DeveloperEvaluation.Application.Queries.Carts.GetActiveCartByUserId;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.AddItem;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.Checkout;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.RemoveItem;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts;

/// <summary>
/// Controller for managing shopping cart operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly ILogger<CartsController> _logger;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the CartsController
    /// </summary>
    /// <param name="mediator">The mediator for handling commands</param>
    /// <param name="logger">The logger for logging messages</param>
    /// <param name="mapper">The AutoMapper instance for object mapping</param>
    public CartsController(IMediator mediator, ILogger<CartsController> logger, IMapper mapper)
    {
        _mediator = mediator;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new shopping cart
    /// </summary>
    /// <param name="request">The cart creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created cart details</returns>
    /// <response code="201">Returns the newly created cart</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateCartResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateCartRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CreateCartCommand>(request);
        var result = await _mediator.Send(command, cancellationToken);

        return Created(string.Empty, new ApiResponseWithData<CreateCartResponse>
        {
            Success = true,
            Message = "Cart created successfully",
            Data = _mapper.Map<CreateCartResponse>(result)
        });
    }

    /// <summary>
    /// Retrieves a cart by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the cart</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The cart details if found</returns>
    /// <response code="200">Returns the requested cart</response>
    /// <response code="404">If the cart is not found</response>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new GetCartRequest { Id = id };
        var validator = new GetCartRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var query = _mapper.Map<GetCartByIdQuery>(request);
        var result = await _mediator.Send(query, cancellationToken);

        return Ok(new ApiResponseWithData<GetCartResponse>
        {
            Success = true,
            Message = "Cart retrieved successfully",
            Data = _mapper.Map<GetCartResponse>(result)
        });
    }

    /// <summary>
    /// Adds or updates an item in a cart
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart</param>
    /// <param name="request">The item addition/update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The added/updated cart item details</returns>
    /// <response code="200">Returns the added/updated cart item</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the cart is not found</response>
    [HttpPost("{cartId}/items")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponseWithData<AddItemResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddItem([FromRoute] Guid cartId, [FromBody] AddItemRequest request, CancellationToken cancellationToken)
    {
        var validator = new AddItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<AddCartItemCommand>(request);
        command.CartId = cartId;
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<AddItemResponse>
        {
            Success = true,
            Message = "Item added/updated successfully",
            Data = _mapper.Map<AddItemResponse>(result)
        });
    }

    /// <summary>
    /// Removes an item from a cart
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart</param>
    /// <param name="itemId">The unique identifier of the item to remove</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success response if the item was removed</returns>
    /// <response code="200">If the item was successfully removed</response>
    /// <response code="404">If the cart or item is not found</response>
    [HttpDelete("{cartId}/items/{itemId}")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveItem([FromRoute] Guid cartId, [FromRoute] Guid itemId, CancellationToken cancellationToken)
    {
        var request = new RemoveItemRequest { ItemId = itemId };
        var validator = new RemoveItemRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<RemoveCartItemCommand>(request);
        command.CartId = cartId;
        await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponse
        {
            Success = true,
            Message = "Item removed successfully"
        });
    }

    /// <summary>
    /// Checks out a cart
    /// </summary>
    /// <param name="cartId">The unique identifier of the cart</param>
    /// <param name="request">The checkout request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    /// <response code="200">Returns the created sale</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the cart is not found</response>
    [HttpPost("{cartId}/checkout")]
    [Authorize(Roles = "Admin,Customer")]
    [ProducesResponseType(typeof(ApiResponseWithData<CheckoutResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Checkout([FromRoute] Guid cartId, [FromBody] CheckoutRequest request, CancellationToken cancellationToken)
    {
        var validator = new CheckoutRequestValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var command = _mapper.Map<CheckoutCartCommand>(request);
        command.CartId = cartId;
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(new ApiResponseWithData<CheckoutResponse>
        {
            Success = true,
            Message = "Cart checked out successfully",
            Data = _mapper.Map<CheckoutResponse>(result)
        });
    }

    /// <summary>
    /// Gets the active cart for a user
    /// </summary>
    [HttpGet("user/{userId}/active")]
    [Authorize(Roles = "Customer,Admin,Manager")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetCartResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetActiveByUserId([FromRoute] Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetActiveCartByUserIdQuery { UserId = userId };
        var result = await _mediator.Send(query, cancellationToken);

        if (result == null)
            return NotFound(new ApiResponse
            {
                Success = false,
                Message = "Active cart not found"
            });

        return Ok(new ApiResponseWithData<GetCartResponse>
        {
            Success = true,
            Message = "Active cart retrieved successfully",
            Data = _mapper.Map<GetCartResponse>(result)
        });
    }
}