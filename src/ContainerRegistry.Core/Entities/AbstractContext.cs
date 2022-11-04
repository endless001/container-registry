using Microsoft.EntityFrameworkCore;

namespace ContainerRegistry.Core.Entities;

public abstract class AbstractContext<TContext> : DbContext, IContext where TContext : DbContext
{
    public AbstractContext(DbContextOptions<TContext> options)
        : base(options)
    {
    }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Repository> Repositories { get; set; }
    public DbSet<RepositoryTag> RepositoryTags { get; set; }
    public DbSet<User> Users { get; set; }
    public Task<int> SaveChangesAsync() => SaveChangesAsync(default);

    public virtual async Task RunMigrationsAsync(CancellationToken cancellationToken)
        => await Database.MigrateAsync(cancellationToken);

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }
}