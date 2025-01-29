using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.GetBranch;

public class GetBranchHandlerTests
{
    private readonly TestBranchRepository _branchRepository;
    private readonly GetBranchHandler _handler;

    public GetBranchHandlerTests()
    {
        _branchRepository = new TestBranchRepository();
        _handler = new GetBranchHandler(
            _branchRepository,
            new Logger<GetBranchHandler>(new LoggerFactory())
        );
    }

    [Fact]
    public async Task Handle_Should_Return_Branch_When_Found()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");
        _branchRepository.SetBranch(branch);

        var query = new GetBranchQuery(branch.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(branch.Id, result.Id);
        Assert.Equal(branch.Name, result.Name);
        Assert.Equal(branch.State, result.State);
        Assert.Equal(branch.IsActive, result.IsActive);
        Assert.Equal(branch.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Branch_Not_Found()
    {
        // Arrange
        var query = new GetBranchQuery(Guid.NewGuid());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(
            () => _handler.Handle(query, CancellationToken.None)
        );
        Assert.Equal("Branch not found", exception.Message);
    }

    private class TestBranchRepository : IBranchRepository
    {
        private Branch? _branch;

        public void SetBranch(Branch branch)
        {
            _branch = branch;
        }

        public Task<Branch> AddAsync(Branch branch)
        {
            _branch = branch;
            return Task.FromResult(branch);
        }

        public Task<Branch?> GetByIdAsync(Guid id)
        {
            return Task.FromResult(_branch?.Id == id ? _branch : null);
        }

        public Task<IEnumerable<Branch>> GetAllAsync()
        {
            return Task.FromResult(_branch != null ? 
                new[] { _branch }.AsEnumerable() : 
                Enumerable.Empty<Branch>());
        }

        public Task<Branch> UpdateAsync(Branch branch)
        {
            _branch = branch;
            return Task.FromResult(branch);
        }

        public Task DeleteAsync(Guid id)
        {
            if (_branch?.Id == id)
                _branch = null;
            return Task.CompletedTask;
        }
    }
} 