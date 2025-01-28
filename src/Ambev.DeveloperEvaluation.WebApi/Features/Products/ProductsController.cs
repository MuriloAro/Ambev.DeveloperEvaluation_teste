using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using FluentValidation;
using Ambev.DeveloperEvaluation.Application.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateProduct;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;
using Ambev.DeveloperEvaluation.Application.Products.ListProducts;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.ListProducts;
using Ambev.DeveloperEvaluation.Application.Products.ActivateProduct;
using Ambev.DeveloperEvaluation.Application.Products.DeactivateProduct;
using Ambev.DeveloperEvaluation.Application.Products.UpdateStock;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.UpdateStock;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public ProductsController(IMediator mediator, IMapper mapper)
    {
        _mediator = mediator;
        _mapper = mapper;
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">The product data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created product ID</returns>
    /// <response code="200">Returns the created product ID</response>
    /// <response code="400">If the product data is invalid</response>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<CreateProductResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = _mapper.Map<CreateProductCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<CreateProductResult>
            {
                Success = true,
                Message = "Product created successfully",
                Data = result
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

    /// <summary>
    /// Updates an existing product
    /// </summary>
    /// <param name="id">The ID of the product to update</param>
    /// <param name="request">The updated product data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The updated product</returns>
    /// <response code="200">Returns the updated product</response>
    /// <response code="400">If the product data is invalid</response>
    /// <response code="404">If the product is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateProductResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = _mapper.Map<UpdateProductCommand>(request);
            command.Id = id;
            
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateProductResult>
            {
                Success = true,
                Message = "Product updated successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
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

    /// <summary>
    /// Gets a product by ID
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The product details</returns>
    /// <response code="200">Returns the product details</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="400">If the product ID is invalid</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponseWithData<GetProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var query = new GetProductQuery { Id = id };
            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<GetProductResponse>(result);

            return Ok(new ApiResponseWithData<GetProductResponse>
            {
                Success = true,
                Message = "Product retrieved successfully",
                Data = response
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
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

    /// <summary>
    /// Lists products with optional filtering and pagination
    /// </summary>
    /// <param name="page">Page number (starts at 1)</param>
    /// <param name="pageSize">Items per page (max 100)</param>
    /// <param name="status">Filter by product status</param>
    /// <param name="category">Filter by product category</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of products</returns>
    /// <response code="200">Returns the list of products</response>
    /// <response code="400">If pagination parameters are invalid</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<ListProductsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] ProductStatus? status = null,
        [FromQuery] ProductCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new ListProductsQuery
            {
                Page = page,
                PageSize = pageSize,
                Status = status,
                Category = category
            };

            var result = await _mediator.Send(query, cancellationToken);
            var response = _mapper.Map<ListProductsResponse>(result);

            return Ok(new ApiResponseWithData<ListProductsResponse>
            {
                Success = true,
                Message = "Products retrieved successfully",
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

    /// <summary>
    /// Activates a product
    /// </summary>
    /// <param name="id">The ID of the product to activate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="400">If the product ID is invalid or product is already active</response>
    [HttpPatch("{id}/activate")]
    [ProducesResponseType(typeof(ApiResponseWithData<ActivateProductResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Activate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new ActivateProductCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<ActivateProductResult>
            {
                Success = true,
                Message = "Product activated successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
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

    /// <summary>
    /// Deactivates a product
    /// </summary>
    /// <param name="id">The ID of the product to deactivate</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="400">If the product ID is invalid or product is already inactive</response>
    [HttpPatch("{id}/deactivate")]
    [ProducesResponseType(typeof(ApiResponseWithData<DeactivateProductResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var command = new DeactivateProductCommand { Id = id };
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<DeactivateProductResult>
            {
                Success = true,
                Message = "Product deactivated successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
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

    /// <summary>
    /// Updates the stock quantity of a product
    /// </summary>
    /// <param name="id">The ID of the product</param>
    /// <param name="request">The new stock quantity</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Success message</returns>
    /// <response code="200">Returns success message</response>
    /// <response code="404">If the product is not found</response>
    /// <response code="400">If the product ID is invalid, product is inactive, or stock quantity is negative</response>
    [HttpPatch("{id}/stock")]
    [ProducesResponseType(typeof(ApiResponseWithData<UpdateStockResult>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStock(Guid id, [FromBody] UpdateStockRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new UpdateStockCommand 
            { 
                Id = id,
                StockQuantity = request.StockQuantity
            };
            
            var result = await _mediator.Send(command, cancellationToken);

            return Ok(new ApiResponseWithData<UpdateStockResult>
            {
                Success = true,
                Message = "Stock updated successfully",
                Data = result
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new ApiResponse 
            { 
                Success = false,
                Message = ex.Message
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