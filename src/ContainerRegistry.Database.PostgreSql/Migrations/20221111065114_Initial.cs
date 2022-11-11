using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContainerRegistry.Database.PostgreSql.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organizations",
                columns: table => new
                {
                    organizationid = table.Column<int>(name: "organization_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    organizationnamespace = table.Column<string>(name: "organization_namespace", type: "character varying(4000)", maxLength: 4000, nullable: false),
                    organizationname = table.Column<string>(name: "organization_name", type: "character varying(4000)", maxLength: 4000, nullable: false),
                    organizationlocation = table.Column<string>(name: "organization_location", type: "character varying(4000)", maxLength: 4000, nullable: false),
                    organizationgravatar = table.Column<string>(name: "organization_gravatar", type: "character varying(4000)", maxLength: 4000, nullable: false),
                    organizationemail = table.Column<string>(name: "organization_email", type: "character varying(4000)", maxLength: 4000, nullable: false),
                    organizationcreated = table.Column<DateTime>(name: "organization_created", type: "timestamp with time zone", nullable: false),
                    organizationupdated = table.Column<DateTime>(name: "organization_updated", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organizations", x => x.organizationid);
                });

            migrationBuilder.CreateTable(
                name: "repository_types",
                columns: table => new
                {
                    repositorytypeid = table.Column<int>(name: "repository_type_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    repositorytypename = table.Column<string>(name: "repository_type_name", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repository_types", x => x.repositorytypeid);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    userid = table.Column<int>(name: "user_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(name: "user_name", type: "text", nullable: false),
                    usersecret = table.Column<string>(name: "user_secret", type: "text", nullable: false),
                    useremail = table.Column<string>(name: "user_email", type: "text", nullable: false),
                    useravatar = table.Column<string>(name: "user_avatar", type: "text", nullable: false),
                    usertoken = table.Column<string>(name: "user_token", type: "text", nullable: false),
                    userrefresh = table.Column<TimeSpan>(name: "user_refresh", type: "interval", nullable: false),
                    userexpiry = table.Column<string>(name: "user_expiry", type: "text", nullable: false),
                    usercreated = table.Column<DateTime>(name: "user_created", type: "timestamp with time zone", nullable: false),
                    userupdated = table.Column<DateTime>(name: "user_updated", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.userid);
                });

            migrationBuilder.CreateTable(
                name: "repositories",
                columns: table => new
                {
                    repositoryid = table.Column<int>(name: "repository_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    repositoryname = table.Column<string>(name: "repository_name", type: "character varying(4000)", maxLength: 4000, nullable: false),
                    repositorydownloads = table.Column<long>(name: "repository_downloads", type: "bigint", nullable: false),
                    organizationid = table.Column<int>(name: "organization_id", type: "integer", nullable: false),
                    repositorytypeid = table.Column<int>(name: "repository_type_id", type: "integer", nullable: false),
                    repositorycreated = table.Column<DateTime>(name: "repository_created", type: "timestamp with time zone", nullable: false),
                    repositoryupdated = table.Column<DateTime>(name: "repository_updated", type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repositories", x => x.repositoryid);
                    table.ForeignKey(
                        name: "FK_repositories_organizations_organization_id",
                        column: x => x.organizationid,
                        principalTable: "organizations",
                        principalColumn: "organization_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_repositories_repository_types_repository_type_id",
                        column: x => x.repositorytypeid,
                        principalTable: "repository_types",
                        principalColumn: "repository_type_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "organization_members",
                columns: table => new
                {
                    organizationId = table.Column<int>(name: "organization_Id", type: "integer", nullable: false),
                    memberid = table.Column<int>(name: "member_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization_members", x => new { x.memberid, x.organizationId });
                    table.ForeignKey(
                        name: "FK_organization_members_organizations_organization_Id",
                        column: x => x.organizationId,
                        principalTable: "organizations",
                        principalColumn: "organization_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_organization_members_users_member_id",
                        column: x => x.memberid,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "repository_tags",
                columns: table => new
                {
                    repositorytagid = table.Column<int>(name: "repository_tag_id", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    repositorytagname = table.Column<string>(name: "repository_tag_name", type: "text", nullable: false),
                    repositoryid = table.Column<int>(name: "repository_id", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_repository_tags", x => x.repositorytagid);
                    table.ForeignKey(
                        name: "FK_repository_tags_repositories_repository_id",
                        column: x => x.repositoryid,
                        principalTable: "repositories",
                        principalColumn: "repository_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_organization_members_organization_Id",
                table: "organization_members",
                column: "organization_Id");

            migrationBuilder.CreateIndex(
                name: "IX_repositories_organization_id",
                table: "repositories",
                column: "organization_id");

            migrationBuilder.CreateIndex(
                name: "IX_repositories_repository_type_id",
                table: "repositories",
                column: "repository_type_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_repository_tags_repository_id",
                table: "repository_tags",
                column: "repository_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "organization_members");

            migrationBuilder.DropTable(
                name: "repository_tags");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "repositories");

            migrationBuilder.DropTable(
                name: "organizations");

            migrationBuilder.DropTable(
                name: "repository_types");
        }
    }
}
