using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.WebApi.Features.Products.CreateProduct;
using FluentValidation;

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
} 