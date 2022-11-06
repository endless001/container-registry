using Microsoft.Extensions.DependencyInjection;

namespace ContainerRegistry.Core;

public class ContainerRegistryBuilder
{
    public ContainerRegistryBuilder(IServiceCollection services)
    {
        Services = services ?? throw new ArgumentNullException(nameof(services));
    }

    public IServiceCollection Services { get; }
}