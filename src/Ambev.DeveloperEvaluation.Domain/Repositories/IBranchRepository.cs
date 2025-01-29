using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories;

public interface IBranchRepository
{
    Task<Branch> AddAsync(Branch branch);
    Task<Branch?> GetByIdAsync(Guid id);
    Task<IEnumerable<Branch>> GetAllAsync();
    Task<Branch> UpdateAsync(Branch branch);
    Task DeleteAsync(Guid id);
} 