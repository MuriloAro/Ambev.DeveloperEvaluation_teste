using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

/// <summary>
/// AutoMapper profile for branch update mappings
/// </summary>
public sealed class UpdateBranchProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for branch update
    /// </summary>
    public UpdateBranchProfile()
    {
        CreateMap<Branch, UpdateBranchResult>();
    }
} 