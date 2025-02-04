using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

/// <summary>
/// AutoMapper profile for branch retrieval mappings
/// </summary>
public sealed class GetBranchProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for branch retrieval
    /// </summary>
    public GetBranchProfile()
    {
        CreateMap<Branch, GetBranchResult>();
    }
} 