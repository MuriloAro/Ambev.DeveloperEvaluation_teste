using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.ORM.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly DefaultContext _context;
    private readonly ILogger<BranchRepository> _logger;

    public BranchRepository(DefaultContext context, ILogger<BranchRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Branch> AddAsync(Branch branch)
    {
        _logger.LogInformation("Adding new branch: {BranchName}", branch.Name);
        await _context.Branches.AddAsync(branch);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Branch added successfully: {BranchId}", branch.Id);
        return branch;
    }

    public async Task<Branch?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting branch by id: {BranchId}", id);
        return await _context.Branches.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<Branch>> GetAllAsync()
    {
        _logger.LogInformation("Getting all branches");
        return await _context.Branches.ToListAsync();
    }

    public async Task<Branch> UpdateAsync(Branch branch)
    {
        _logger.LogInformation("Updating branch: {BranchId}", branch.Id);
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Branch updated successfully: {BranchId}", branch.Id);
        return branch;
    }

    public async Task DeleteAsync(Guid id)
    {
        _logger.LogInformation("Deleting branch: {BranchId}", id);
        var branch = await GetByIdAsync(id);
        if (branch != null)
        {
            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Branch deleted successfully: {BranchId}", id);
        }
    }
} 