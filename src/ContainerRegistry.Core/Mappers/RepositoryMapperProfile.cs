using AutoMapper;
using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Mappers;

public class RepositoryMapperProfile : Profile
{
    public RepositoryMapperProfile()
    {
        CreateMap<Repository, RepositoryResponse>();
    }
}