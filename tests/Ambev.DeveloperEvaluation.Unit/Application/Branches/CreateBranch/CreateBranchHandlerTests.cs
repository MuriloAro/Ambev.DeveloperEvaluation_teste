using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.CreateBranch;

public class CreateBranchHandlerTests
{
    private readonly TestBranchRepository _branchRepository;
    private readonly CreateBranchHandler _handler;

    public CreateBranchHandlerTests()
    {
        _branchRepository = new TestBranchRepository();
        _handler = new CreateBranchHandler(
            _branchRepository,
            new Logger<CreateBranchHandler>(new LoggerFactory())
        );
    }

    [Fact]
    public async Task Handle_Should_Create_Branch_Successfully()
    {
        // Arrange
        var command = new CreateBranchCommand("São Paulo - Centro", "SP");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.State, result.State);
        Assert.True(result.IsActive);
        Assert.NotEqual(default, result.CreatedAt);
    }

    [Theory]
    [InlineData("", "SP")]
    [InlineData(null, "SP")]
    [InlineData("São Paulo - Centro", "")]
    [InlineData("São Paulo - Centro", null)]
    [InlineData("São Paulo - Centro", "S")]
    [InlineData("São Paulo - Centro", "SPP")]
    public async Task Handle_Should_Throw_When_Invalid_Data(string? name, string? state)
    {
        // Arrange
        var command = new CreateBranchCommand(name!, state!);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
    }

    private class TestBranchRepository : IBranchRepository
    {
        private Branch? _branch;

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