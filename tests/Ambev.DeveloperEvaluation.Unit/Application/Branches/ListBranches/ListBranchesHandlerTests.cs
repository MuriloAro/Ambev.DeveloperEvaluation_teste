using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.ListBranches;

public class ListBranchesHandlerTests
{
    private readonly TestBranchRepository _branchRepository;
    private readonly ListBranchesHandler _handler;

    public ListBranchesHandlerTests()
    {
        _branchRepository = new TestBranchRepository();
        _handler = new ListBranchesHandler(
            _branchRepository,
            new Logger<ListBranchesHandler>(new LoggerFactory())
        );
    }

    [Fact]
    public async Task Handle_Should_Return_All_Branches()
    {
        // Arrange
        var branch1 = new Branch("São Paulo - Centro", "SP");
        var branch2 = new Branch("Rio de Janeiro - Centro", "RJ");
        
        _branchRepository.AddBranch(branch1);
        _branchRepository.AddBranch(branch2);

        // Act
        var result = await _handler.Handle(new ListBranchesQuery(), CancellationToken.None);

        // Assert
        var branches = result.ToList();
        Assert.Equal(2, branches.Count);
        
        Assert.Contains(branches, b => 
            b.Id == branch1.Id && 
            b.Name == branch1.Name && 
            b.State == branch1.State &&
            b.IsActive == branch1.IsActive &&
            b.CreatedAt == branch1.CreatedAt);
            
        Assert.Contains(branches, b => 
            b.Id == branch2.Id && 
            b.Name == branch2.Name && 
            b.State == branch2.State &&
            b.IsActive == branch2.IsActive &&
            b.CreatedAt == branch2.CreatedAt);
    }

    [Fact]
    public async Task Handle_Should_Return_Empty_When_No_Branches()
    {
        // Act
        var result = await _handler.Handle(new ListBranchesQuery(), CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    private class TestBranchRepository : IBranchRepository
    {
        private readonly List<Branch> _branches = new();

        public void AddBranch(Branch branch)
        {
            _branches.Add(branch);
        }

        public Task<Branch> AddAsync(Branch branch)
        {
            _branches.Add(branch);
            return Task.FromResult(branch);
        }

        public Task<Branch?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_branches.FirstOrDefault(b => b.Id == id));
        }

        public Task<IEnumerable<Branch>> GetAllAsync()
        {
            return Task.FromResult(_branches.AsEnumerable());
        }

        public Task<Branch> UpdateAsync(Branch branch)
        {
            var index = _branches.FindIndex(b => b.Id == branch.Id);
            if (index >= 0)
                _branches[index] = branch;
            return Task.FromResult(branch);
        }

        public Task DeleteAsync(Guid id)
        {
            _branches.RemoveAll(b => b.Id == id);
            return Task.CompletedTask;
        }
    }
} 