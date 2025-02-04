using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.UpdateBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.UpdateBranch;

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
        CreateMap<UpdateBranchRequest, UpdateBranchCommand>();
        CreateMap<UpdateBranchResult, UpdateBranchResponse>();
    }
} 