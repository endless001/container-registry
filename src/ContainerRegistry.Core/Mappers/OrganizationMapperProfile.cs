using AutoMapper;
using ContainerRegistry.Core.Entities;
using ContainerRegistry.Core.Models;

namespace ContainerRegistry.Core.Mappers;

public class OrganizationMapperProfile : Profile
{
    public OrganizationMapperProfile()
    {
        CreateMap<Organization, OrganizationResponse>();
        CreateMap<OrganizationRequest, Organization>();
    }
}