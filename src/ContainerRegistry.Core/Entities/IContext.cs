using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ContainerRegistry.Core.Entities;

public interface IContext
{
    DatabaseFacade Database { get; }
    DbSet<Organization> Organizations { get; set; }
    DbSet<OrganizationMember> OrganizationMembers { get; set; }
    DbSet<Repository> Repositories { get; set; }
    DbSet<RepositoryTag> RepositoryTags { get; set; }
    DbSet<User> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task RunMigrationsAsync(CancellationToken cancellationToken);
}