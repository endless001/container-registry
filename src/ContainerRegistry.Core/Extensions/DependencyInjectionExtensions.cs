using Microsoft.Extensions.DependencyInjection;

namespace ContainerRegistry.Core.Extensions;

public static partial class DependencyInjectionExtensions
{
    private static ContainerRegistryBuilder AddContainerRegistryBuilder(this IServiceCollection services)
    {
        return new ContainerRegistryBuilder(services);
    }

    public static ContainerRegistryBuilder AddContainerRegistry(this IServiceCollection services)
    {
        var builder = services.AddContainerRegistryBuilder();
        return builder;
    }
}