using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

/// <summary>
/// AutoMapper profile for branch creation mappings
/// </summary>
public sealed class CreateBranchProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for branch creation
    /// </summary>
    public CreateBranchProfile()
    {
        CreateMap<Branch, CreateBranchResult>();
    }
} 