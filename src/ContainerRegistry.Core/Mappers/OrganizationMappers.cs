using AutoMapper;

namespace ContainerRegistry.Core.Mappers;

public static class OrganizationMappers
{
    static OrganizationMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<OrganizationMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    public static T ToOrganizationModel<T>(this object source)
    {
        return Mapper.Map<T>(source);
    }
}