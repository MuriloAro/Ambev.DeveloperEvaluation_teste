using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface ICartRepository
{
    Task<Cart> GetByIdAsync(Guid id);
    Task<Cart> GetActiveCartByUserIdAsync(Guid userId);
    Task<Cart> AddAsync(Cart cart);
    Task UpdateAsync(Cart cart);
    Task DeleteAsync(Guid id);
} 