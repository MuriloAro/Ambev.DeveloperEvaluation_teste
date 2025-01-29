using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Logging;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

namespace Ambev.DeveloperEvaluation.Unit.Application.Branches.UpdateBranch;

public class UpdateBranchHandlerTests
{
    private readonly TestBranchRepository _branchRepository;
    private readonly UpdateBranchHandler _handler;

    public UpdateBranchHandlerTests()
    {
        _branchRepository = new TestBranchRepository();
        _handler = new UpdateBranchHandler(
            _branchRepository,
            new Logger<UpdateBranchHandler>(new LoggerFactory())
        );
    }

    [Fact]
    public async Task Handle_Should_Update_Branch_Successfully()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");
        _branchRepository.SetBranch(branch);

        var command = new UpdateBranchCommand(
            branch.Id,
            "São Paulo - Zona Sul",
            "SP",
            true
        );

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Id, result.Id);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.State, result.State);
        Assert.Equal(command.IsActive, result.IsActive);
        Assert.NotEqual(default, result.UpdatedAt);
    }

    [Fact]
    public async Task Handle_Should_Throw_When_Branch_Not_Found()
    {
        // Arrange
        var command = new UpdateBranchCommand(
            Guid.NewGuid(),
            "São Paulo - Zona Sul",
            "SP",
            true
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
        Assert.Equal("Branch not found", exception.Message);
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
        var branch = new Branch("São Paulo - Centro", "SP");
        _branchRepository.SetBranch(branch);

        var command = new UpdateBranchCommand(
            branch.Id,
            name!,
            state!,
            true
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DomainException>(
            () => _handler.Handle(command, CancellationToken.None)
        );
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