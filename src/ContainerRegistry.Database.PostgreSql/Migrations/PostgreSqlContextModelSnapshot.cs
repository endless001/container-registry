﻿// <auto-generated />
using System;
using ContainerRegistry.Database.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContainerRegistry.Database.PostgreSql.Migrations
{
    [DbContext(typeof(PostgreSqlContext))]
    partial class PostgreSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc.2.22472.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ContainerRegistry.Core.Entities.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("organization_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("organization_created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("organization_email");

                    b.Property<string>("Gravatar")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("organization_gravatar");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("organization_location");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("organization_name");

                    b.Property<string>("Namespace")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("organization_namespace");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("organization_updated");

                    b.HasKey("Id");

                    b.ToTable("organizations", (string)null);
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.OrganizationMember", b =>
                {
                    b.Property<int>("MemberId")
                        .HasColumnType("integer")
                        .HasColumnName("member_id");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("integer")
                        .HasColumnName("organization_Id");

                    b.HasKey("MemberId", "OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("organization_members", (string)null);
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.Repository", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("repository_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("repository_created");

                    b.Property<long>("Downloads")
                        .HasColumnType("bigint")
                        .HasColumnName("repository_downloads");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("character varying(4000)")
                        .HasColumnName("repository_name");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("integer")
                        .HasColumnName("organization_id");

                    b.Property<int>("RepositoryTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("repository_type_id");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("repository_updated");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("RepositoryTypeId")
                        .IsUnique();

                    b.ToTable("repositories", (string)null);
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.RepositoryTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("repository_tag_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("repository_tag_name");

                    b.Property<int>("RepositoryId")
                        .HasColumnType("integer")
                        .HasColumnName("repository_id");

                    b.HasKey("Id");

                    b.HasIndex("RepositoryId");

                    b.ToTable("repository_tags", (string)null);
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.RepositoryType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("repository_type_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("repository_type_name");

                    b.HasKey("Id");

                    b.ToTable("repository_types", (string)null);
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Avatar")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_avatar");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("user_created");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_email");

                    b.Property<TimeSpan?>("Expiry")
                        .HasColumnType("interval")
                        .HasColumnName("user_expiry");

                    b.Property<string>("Refresh")
                        .HasColumnType("text")
                        .HasColumnName("user_refresh");

                    b.Property<string>("Secret")
                        .HasColumnType("text")
                        .HasColumnName("user_secret");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_token");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("user_updated");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_name");

                    b.HasKey("Id");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.OrganizationMember", b =>
                {
                    b.HasOne("ContainerRegistry.Core.Entities.User", "User")
                        .WithMany("OrganizationMembers")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContainerRegistry.Core.Entities.Organization", "Organization")
                        .WithMany("OrganizationMembers")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.Repository", b =>
                {
                    b.HasOne("ContainerRegistry.Core.Entities.Organization", "Organization")
                        .WithMany("Repositories")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ContainerRegistry.Core.Entities.RepositoryType", "Type")
                        .WithOne("Repository")
                        .HasForeignKey("ContainerRegistry.Core.Entities.Repository", "RepositoryTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.RepositoryTag", b =>
                {
                    b.HasOne("ContainerRegistry.Core.Entities.Repository", "Repository")
                        .WithMany("Tags")
                        .HasForeignKey("RepositoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Repository");
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.Organization", b =>
                {
                    b.Navigation("OrganizationMembers");

                    b.Navigation("Repositories");
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.Repository", b =>
                {
                    b.Navigation("Tags");
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.RepositoryType", b =>
                {
                    b.Navigation("Repository")
                        .IsRequired();
                });

            modelBuilder.Entity("ContainerRegistry.Core.Entities.User", b =>
                {
                    b.Navigation("OrganizationMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
