using Xunit;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class BranchTests
{
    [Fact]
    public void Should_Create_Branch_With_Valid_Data()
    {
        // Arrange
        var name = "São Paulo - Centro";
        var state = "SP";

        // Act
        var branch = new Branch(name, state);

        // Assert
        Assert.NotEqual(Guid.Empty, branch.Id);
        Assert.Equal(name, branch.Name);
        Assert.Equal(state, branch.State);
        Assert.True(branch.IsActive);
        Assert.NotEqual(default, branch.CreatedAt);
        Assert.Null(branch.UpdatedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void Should_Throw_DomainException_When_Name_Is_Invalid(string? invalidName)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Branch(invalidName!, "SP"));
        Assert.Equal("Branch name cannot be empty", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("SPP")]
    [InlineData("S")]
    public void Should_Throw_DomainException_When_State_Is_Invalid(string? invalidState)
    {
        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Branch("São Paulo - Centro", invalidState!));
        Assert.Equal("State must be exactly 2 characters", exception.Message);
    }

    [Fact]
    public void Should_Deactivate_Branch()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");

        // Act
        branch.Deactivate();

        // Assert
        Assert.False(branch.IsActive);
        Assert.NotNull(branch.UpdatedAt);
    }

    [Fact]
    public void Should_Not_Deactivate_Already_Inactive_Branch()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");
        branch.Deactivate();
        var firstUpdateAt = branch.UpdatedAt;

        // Act
        branch.Deactivate();

        // Assert
        Assert.False(branch.IsActive);
        Assert.Equal(firstUpdateAt, branch.UpdatedAt);
    }

    [Fact]
    public void Should_Activate_Branch()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");
        branch.Deactivate();

        // Act
        branch.Activate();

        // Assert
        Assert.True(branch.IsActive);
        Assert.NotNull(branch.UpdatedAt);
    }

    [Fact]
    public void Should_Not_Activate_Already_Active_Branch()
    {
        // Arrange
        var branch = new Branch("São Paulo - Centro", "SP");
        var initialUpdateAt = branch.UpdatedAt;

        // Act
        branch.Activate();

        // Assert
        Assert.True(branch.IsActive);
        Assert.Equal(initialUpdateAt, branch.UpdatedAt);
    }
}