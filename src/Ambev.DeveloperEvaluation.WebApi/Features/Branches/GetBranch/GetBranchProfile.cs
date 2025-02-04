using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.GetBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.GetBranch;

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
        CreateMap<GetBranchRequest, GetBranchCommand>();
        CreateMap<GetBranchResult, GetBranchResponse>();
    }
} 