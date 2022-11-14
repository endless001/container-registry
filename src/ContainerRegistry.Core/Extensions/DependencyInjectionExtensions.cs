using ContainerRegistry.Core.Configuration;
using ContainerRegistry.Core.Services;
using ContainerRegistry.Core.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

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
        services.AddContainerRegistryServices();
        services.AddConfiguration();
        return builder;
    }

    public static IServiceCollection AddContainerRegistryOptions<TOptions>(
        this IServiceCollection services,
        string key = null)
        where TOptions : class
    {
        services.AddSingleton<IConfigureOptions<TOptions>>(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            if (key != null)
            {
                config = config.GetSection(key);
            }

            return new BindOptions<TOptions>(config);
        });

        return services;
    }

    private static void AddConfiguration(this IServiceCollection services)
    {
        services.AddContainerRegistryOptions<ContainerRegistryOptions>();
        services.AddContainerRegistryOptions<DatabaseOptions>(nameof(ContainerRegistryOptions.Database));
        services.AddContainerRegistryOptions<JwtBearerOptions>(nameof(ContainerRegistryOptions.JwtBearer));
    }

    private static void AddContainerRegistryServices(this IServiceCollection services)
    {
        services.TryAddTransient<IRegistryStorageService, RegistryStorageService>();
        services.TryAddTransient<IUserService, UserService>();
        services.TryAddTransient<IOrganizationService, OrganizationService>();
        services.TryAddTransient<IConnectService, ConnectService>();
    }
}