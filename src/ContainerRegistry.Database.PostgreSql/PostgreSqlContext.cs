using ContainerRegistry.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace ContainerRegistry.Database.PostgreSql;

public class PostgreSqlContext : AbstractContext<PostgreSqlContext>
{
    public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
    {
    }

    public override async Task RunMigrationsAsync(CancellationToken cancellationToken)
    {
        await base.RunMigrationsAsync(cancellationToken);

        if (Database.GetDbConnection() is NpgsqlConnection connection)
        {
            await connection.OpenAsync(cancellationToken);
            connection.ReloadTypes();
        }
    }
}