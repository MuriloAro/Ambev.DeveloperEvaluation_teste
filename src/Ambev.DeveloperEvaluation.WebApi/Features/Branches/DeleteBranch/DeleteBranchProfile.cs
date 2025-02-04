using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Branches.DeleteBranch;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Branches.DeleteBranch;

/// <summary>
/// AutoMapper profile for branch deletion mappings
/// </summary>
public sealed class DeleteBranchProfile : Profile
{
    /// <summary>
    /// Initializes mapping configuration for branch deletion
    /// </summary>
    public DeleteBranchProfile()
    {
        CreateMap<DeleteBranchRequest, DeleteBranchCommand>();
    }
} 