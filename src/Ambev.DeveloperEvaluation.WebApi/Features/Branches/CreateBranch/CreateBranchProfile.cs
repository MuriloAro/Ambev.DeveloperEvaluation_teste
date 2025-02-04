using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.CreateBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.CreateBranch;

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
        CreateMap<CreateBranchRequest, CreateBranchCommand>();
        CreateMap<CreateBranchResult, CreateBranchResponse>();
    }
} 