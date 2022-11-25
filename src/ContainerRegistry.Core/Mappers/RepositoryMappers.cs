using AutoMapper;

namespace ContainerRegistry.Core.Mappers;

public static class RepositoryMappers
{
    static RepositoryMappers()
    {
        Mapper = new MapperConfiguration(cfg => cfg.AddProfile<RepositoryMapperProfile>())
            .CreateMapper();
    }

    internal static IMapper Mapper { get; }

    public static T ToRepositoryModel<T>(this object source)
    {
        return Mapper.Map<T>(source);
    }
}