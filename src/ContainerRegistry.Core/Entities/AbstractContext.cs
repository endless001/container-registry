using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ContainerRegistry.Core.Entities;

public abstract class AbstractContext<TContext> : DbContext, IContext where TContext : DbContext
{
    public const int DefaultMaxStringLength = 4000;

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
        builder.Entity<OrganizationMember>(BuildOrganizationMemberEntity);
        builder.Entity<User>(BuildUserEntity);
        builder.Entity<Repository>(BuildRepositoryEntity);
        builder.Entity<Organization>(BuildOrganizationEntity);
        builder.Entity<RepositoryType>(BuildRepositoryTypeEntity);
        builder.Entity<RepositoryTag>(BuildRepositoryTagEntity);
    }

    private void BuildRepositoryTypeEntity(EntityTypeBuilder<RepositoryType> builder)
    {
        builder.ToTable("repository_types");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("repository_type_id");
        
        builder.Property(t => t.Name)
            .HasColumnName("repository_type_name");
    }

    private void BuildRepositoryTagEntity(EntityTypeBuilder<RepositoryTag> builder)
    {
        builder.ToTable("repository_tags");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("repository_tag_id");

        builder.Property(t => t.Name)
            .HasColumnName("repository_tag_name");

        builder.Property(t => t.RepositoryId)
            .HasColumnName("repository_id");

        builder.HasOne(r => r.Repository)
            .WithMany(d => d.Tags)
            .HasForeignKey(k => k.RepositoryId);
    }

    private void BuildOrganizationMemberEntity(EntityTypeBuilder<OrganizationMember> builder)
    {
        builder.ToTable("organization_members");

        builder.HasKey(om => new { om.MemberId, om.OrganizationId });
        
        builder.Property(om => om.MemberId)
            .HasColumnName("member_id");
        
        builder.Property(om => om.OrganizationId)
            .HasColumnName("organization_Id");

        builder.HasOne(om => om.Organization)
            .WithMany(o => o.OrganizationMembers)
            .HasForeignKey(om => om.OrganizationId);

        builder.HasOne(om => om.User)
            .WithMany(u => u.OrganizationMembers)
            .HasForeignKey(om => om.MemberId);
    }

    private void BuildUserEntity(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasColumnName("user_id");

        builder.Property(u => u.UserName)
            .HasColumnName("user_name");

        builder.Property(u => u.Secret)
            .HasColumnName("user_secret");

        builder.Property(u => u.Email)
            .HasColumnName("user_email");

        builder.Property(u => u.Avatar)
            .HasColumnName("user_avatar");

        builder.Property(u => u.Token)
            .HasColumnName("user_token");

        builder.Property(u => u.Refresh)
            .HasColumnName("user_refresh");

        builder.Property(u => u.Expiry)
            .HasColumnName("user_expiry");

        builder.Property(u => u.Created)
            .HasColumnName("user_created");

        builder.Property(u => u.Updated)
            .HasColumnName("user_updated");
    }

    private void BuildRepositoryEntity(EntityTypeBuilder<Repository> builder)
    {
        builder.ToTable("repositories");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName("repository_id");

        builder.Property(r => r.Name)
            .HasColumnName("repository_name")
            .HasMaxLength(DefaultMaxStringLength);

        builder.Property(r => r.Downloads)
            .HasColumnName("repository_downloads");

        builder.Property(r => r.OrganizationId)
            .HasColumnName("organization_id");
        
        builder.Property(r => r.RepositoryTypeId)
            .HasColumnName("repository_type_id");
        
        builder.HasOne(r => r.Organization)
            .WithMany(d => d.Repositories)
            .HasForeignKey(l => l.OrganizationId);

        builder.HasOne(r => r.Type)
            .WithOne(d => d.Repository)
            .HasForeignKey<Repository>(l => l.RepositoryTypeId);

        builder.Property(r => r.Created)
            .HasColumnName("repository_created");

        builder.Property(r => r.Updated)
            .HasColumnName("repository_updated");
    }

    private void BuildOrganizationEntity(EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("organizations");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasColumnName("organization_id");

        builder.Property(o => o.Namespace)
            .HasColumnName("organization_namespace")
            .HasMaxLength(DefaultMaxStringLength);

        builder.Property(o => o.Name)
            .HasColumnName("organization_name")
            .HasMaxLength(DefaultMaxStringLength);

        builder.Property(o => o.Location)
            .HasColumnName("organization_location")
            .HasMaxLength(DefaultMaxStringLength);

        builder.Property(o => o.Gravatar)
            .HasColumnName("organization_gravatar")
            .HasMaxLength(DefaultMaxStringLength);

        builder.Property(o => o.Email)
            .HasColumnName("organization_email")
            .HasMaxLength(DefaultMaxStringLength);

        builder.Property(o => o.Created)
            .HasColumnName("organization_created");

        builder.Property(o => o.Updated)
            .HasColumnName("organization_updated");
    }
}