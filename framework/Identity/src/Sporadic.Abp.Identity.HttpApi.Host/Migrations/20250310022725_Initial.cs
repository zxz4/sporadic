using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AbpPermissionGrants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    TenantId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissionGrants", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AbpPermissionGroups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissionGroups", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AbpPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    GroupName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ParentName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DisplayName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MultiTenancySide = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Providers = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StateCheckers = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExtraProperties = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbpPermissions", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_OrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ParentId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Longitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Code = table.Column<string>(type: "varchar(63)", maxLength: 63, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_OrganizationUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_OrganizationUnits_SYS_OrganizationUnits_ParentId",
                        column: x => x.ParentId,
                        principalTable: "SYS_OrganizationUnits",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(68)", maxLength: 68, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedName = table.Column<string>(type: "varchar(68)", maxLength: 68, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDefault = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsStatic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPublic = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    Email = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    PhoneNumber = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    UserName = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SecurityStamp = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsExternal = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_RoleClaims",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ClaimType = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "varchar(1024)", maxLength: 1024, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SYS_RoleClaims_SYS_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SYS_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_UserLogins",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LoginProvider = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderKey = table.Column<string>(type: "varchar(196)", maxLength: 196, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProviderDisplayName = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserLogins", x => new { x.UserId, x.LoginProvider });
                    table.ForeignKey(
                        name: "FK_SYS_UserLogins_SYS_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SYS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_UserOrganizationUnits",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    OrganizationUnitId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserOrganizationUnits", x => new { x.OrganizationUnitId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SYS_UserOrganizationUnits_SYS_OrganizationUnits_Organization~",
                        column: x => x.OrganizationUnitId,
                        principalTable: "SYS_OrganizationUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_UserOrganizationUnits_SYS_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SYS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SYS_UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    RoleId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_SYS_UserRoles_SYS_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SYS_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SYS_UserRoles_SYS_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "SYS_Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGrants_TenantId_Name_ProviderName_ProviderKey",
                table: "AbpPermissionGrants",
                columns: new[] { "TenantId", "Name", "ProviderName", "ProviderKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissionGroups_Name",
                table: "AbpPermissionGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_GroupName",
                table: "AbpPermissions",
                column: "GroupName");

            migrationBuilder.CreateIndex(
                name: "IX_AbpPermissions_Name",
                table: "AbpPermissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SYS_OrganizationUnits_Code",
                table: "SYS_OrganizationUnits",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_OrganizationUnits_ParentId",
                table: "SYS_OrganizationUnits",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_RoleClaims_RoleId",
                table: "SYS_RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserLogins_LoginProvider_ProviderKey",
                table: "SYS_UserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserOrganizationUnits_UserId_OrganizationUnitId",
                table: "SYS_UserOrganizationUnits",
                columns: new[] { "UserId", "OrganizationUnitId" });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_UserRoles_RoleId_UserId",
                table: "SYS_UserRoles",
                columns: new[] { "RoleId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_SYS_Users_PhoneNumber_Email_UserName",
                table: "SYS_Users",
                columns: new[] { "PhoneNumber", "Email", "UserName" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpPermissionGrants");

            migrationBuilder.DropTable(
                name: "AbpPermissionGroups");

            migrationBuilder.DropTable(
                name: "AbpPermissions");

            migrationBuilder.DropTable(
                name: "SYS_RoleClaims");

            migrationBuilder.DropTable(
                name: "SYS_UserLogins");

            migrationBuilder.DropTable(
                name: "SYS_UserOrganizationUnits");

            migrationBuilder.DropTable(
                name: "SYS_UserRoles");

            migrationBuilder.DropTable(
                name: "SYS_OrganizationUnits");

            migrationBuilder.DropTable(
                name: "SYS_Roles");

            migrationBuilder.DropTable(
                name: "SYS_Users");
        }
    }
}
