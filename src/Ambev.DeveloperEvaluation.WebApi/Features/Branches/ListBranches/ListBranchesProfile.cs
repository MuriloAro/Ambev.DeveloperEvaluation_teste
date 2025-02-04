using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.ListBranches;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.ListBranches;

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
        CreateMap<ListBranchesRequest, ListBranchesQuery>();
        CreateMap<ListBranchesResult, ListBranchesResponse>();
        //CreateMap<BranchItemResult, BranchDto>();
    }
} 