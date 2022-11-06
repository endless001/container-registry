using ContainerRegistry.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Database.PostgreSql;

public class PostgreSqlContext : AbstractContext<PostgreSqlContext>
{
    public PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : base(options)
    {
    }
}