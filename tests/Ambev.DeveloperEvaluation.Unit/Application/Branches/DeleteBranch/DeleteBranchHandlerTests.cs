using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.DeleteBranch;

public class DeleteBranchHandlerTests
{
    private readonly TestBranchRepository _branchRepository;
    private readonly DeleteBranchHandler _handler;

    public DeleteBranchHandlerTests()
    {
        _branchRepository = new TestBranchRepository();
        _handler = new DeleteBranchHandler(
            _branchRepository,
            new Logger<DeleteBranchHandler>(new LoggerFactory())
        );
    }

    [Fact]
    public async Task Handle_Should_Delete_Branch_Successfully()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");
        _branchRepository.SetBranch(branch);

        var command = new DeleteBranchCommand(branch.Id);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        var deletedBranch = await _branchRepository.GetByIdAsync(branch.Id);
        Assert.Null(deletedBranch);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Branch_Not_Found()
    {
        // Arrange
        var command = new DeleteBranchCommand(Guid.NewGuid());

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(
            () => _handler.Handle(command, CancellationToken.None)
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
            if (_branch?.Id == branch.Id)
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