using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

/// <summary>
/// AutoMapper profile for branch listing mappings
/// </summary>
public sealed class ListBranchesProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for branch listing
    /// </summary>
    public ListBranchesProfile()
    {
        CreateMap<Branch, BranchItemResult>();
    }
} 