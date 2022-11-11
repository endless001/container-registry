using ContainerRegistry.Core.Configuration;
using ContainerRegistry.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ContainerRegistry.Core.Extensions;

public static partial class DependencyInjectionExtensions
{
    private static readonly string DatabaseTypeKey = $"{nameof(DatabaseOptions.Type)}";
    private static readonly string StorageTypeKey = $"{nameof(StorageOptions.Type)}";


    public static IServiceCollection AddDatabaseContextProvider<TContext>(
        this IServiceCollection services,
        string databaseType,
        Action<IServiceProvider, DbContextOptionsBuilder> configureContext)
        where TContext : DbContext, IContext
    {
        services.TryAddScoped<IContext>(provider => provider.GetRequiredService<TContext>());

        services.AddDbContext<TContext>(configureContext);
        services.AddProvider<IContext>((provider, config) =>
            !config.HasDatabaseType(databaseType) ? null : provider.GetRequiredService<TContext>());
        return services;
    }

    public static IServiceCollection AddProvider<TService>(
        this IServiceCollection services,
        Func<IServiceProvider, IConfiguration, TService> func)
        where TService : class
    {
        services.AddSingleton<IProvider<TService>>(new DelegateProvider<TService>(func));

        return services;
    }

    public static bool HasStorageType(this IConfiguration config, string value)
    {
        return config[StorageTypeKey].Equals(value, StringComparison.OrdinalIgnoreCase);
    }
    public static bool HasDatabaseType(this IConfiguration config, string value)
    {
        return config[DatabaseTypeKey].Equals(value, StringComparison.OrdinalIgnoreCase);
    }
    public static TService GetServiceFromProviders<TService>(IServiceProvider services)
        where TService : class
    {
        var providers = services.GetRequiredService<IEnumerable<IProvider<TService>>>();
        var configuration = services.GetRequiredService<IConfiguration>();

        return providers.Select(provider => provider.GetOrNull(services, configuration))
            .FirstOrDefault(result => result != null);
    }
}