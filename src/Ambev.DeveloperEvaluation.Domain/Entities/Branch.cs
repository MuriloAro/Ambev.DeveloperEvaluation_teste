using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Exceptions;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Branch : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    protected Branch() { } // Para o EF Core

    public Branch(string name, string state)
    {
        ValidateName(name);
        ValidateState(state);

        Id = Guid.NewGuid();
        Name = name;
        State = state;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Branch name cannot be empty");
    }

    private static void ValidateState(string state)
    {
        if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
            throw new DomainException("State must be exactly 2 characters");
    }

    public void Activate()
    {
        if (IsActive)
            return;

        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            return;

        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string state)
    {
        ValidateName(name);
        ValidateState(state);

        Name = name;
        State = state;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetActive(bool isActive)
    {
        if (IsActive == isActive)
            return;

        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }
} 