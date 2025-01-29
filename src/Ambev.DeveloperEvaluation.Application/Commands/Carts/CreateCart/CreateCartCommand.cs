using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Application.DTOs;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Commands.Carts.CreateCart
{
    public class CreateCartCommand : IRequest<CartDto>
    {
        public Guid UserId { get; set; }
    }

    public class CreateCartCommandValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required");
        }
    }

    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, CartDto>
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CreateCartCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateCartCommandHandler(
            ICartRepository cartRepository,
            IUserRepository userRepository,
            ILogger<CreateCartCommandHandler> logger,
            IMapper mapper)
        {
            _cartRepository = cartRepository;
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<CartDto> Handle(CreateCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating cart for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new DomainException("User not found");
            }

            var existingCart = await _cartRepository.GetActiveCartByUserIdAsync(request.UserId);
            if (existingCart != null)
            {
                return _mapper.Map<CartDto>(existingCart);
            }

            var cart = new Cart(request.UserId);
            await _cartRepository.AddAsync(cart);

            _logger.LogInformation("Cart {CartId} created successfully", cart.Id);

            return _mapper.Map<CartDto>(cart);
        }
    }
} 