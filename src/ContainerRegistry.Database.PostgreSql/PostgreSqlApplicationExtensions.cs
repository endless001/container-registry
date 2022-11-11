using ContainerRegistry.Core;
using ContainerRegistry.Core.Configuration;
using ContainerRegistry.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ContainerRegistry.Database.PostgreSql;

public static class PostgreSqlApplicationExtensions
{
    public static ContainerRegistryBuilder AddPostgreSqlDatabase(this ContainerRegistryBuilder builder)
    {
        builder.Services.AddDatabaseContextProvider<PostgreSqlContext>("PostgreSql", (provider, options) =>
        {
            var databaseOptions = provider.GetRequiredService<IOptionsSnapshot<DatabaseOptions>>();
            options.UseNpgsql(databaseOptions.Value.ConnectionString);
        });

        return builder;
    }

    public static ContainerRegistryBuilder AddPostgreSqlDatabase(
        this ContainerRegistryBuilder builder,
        Action<DatabaseOptions> configure)
    {
        builder.AddPostgreSqlDatabase();
        builder.Services.Configure(configure);
        return builder;
    }
}