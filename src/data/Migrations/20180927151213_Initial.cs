using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ProviderId = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Provider", x => x.ProviderId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CultureName = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    TimeZoneId = table.Column<string>(maxLength: 32, nullable: false),
                    Username = table.Column<string>(maxLength: 128, nullable: false),
                    CreatedBy = table.Column<long>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedOn = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedBy = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_User_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_User_User_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 32, nullable: false),
                    ParentRoleId = table.Column<string>(maxLength: 32, nullable: true),
                    Enabled = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(maxLength: 64, nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedBy = table.Column<long>(nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleId", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Role_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_User_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Role_Role_ParentRoleId",
                        column: x => x.ParentRoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecurityClaim",
                columns: table => new
                {
                    SecurityClaimId = table.Column<string>(maxLength: 32, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: false),
                    Enabled = table.Column<bool>(nullable: false),
                    Origin = table.Column<string>(maxLength: 32, nullable: false),
                    ValidationPattern = table.Column<string>(maxLength: 256, nullable: false),
                    CreatedBy = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedBy = table.Column<long>(nullable: true),
                    LastUpdatedOn = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecurityClaim", x => x.SecurityClaimId);
                    table.ForeignKey(
                        name: "FK_SecurityClaim_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SecurityClaim_User_LastUpdatedBy",
                        column: x => x.LastUpdatedBy,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserProvider",
                columns: table => new
                {
                    ProviderId = table.Column<string>(maxLength: 64, nullable: false),
                    UserId = table.Column<long>(nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ExternalId = table.Column<string>(maxLength: 64, nullable: true),
                    UserProviderType = table.Column<string>(nullable: false),
                    PasswordSalt = table.Column<string>(maxLength: 128, nullable: true),
                    PasswordHash = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProvider", x => new { x.ProviderId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserProvider_Provider",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "ProviderId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserProvider_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Verification",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    Fingerprint = table.Column<string>(maxLength: 256, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 64, nullable: false),
                    IssuedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    RedeemedAt = table.Column<DateTime>(type: "DATETIME2(7)", nullable: true),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verification", x => x.Code);
                    table.ForeignKey(
                        name: "FK_Verification_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 32, nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RoleId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRole_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoleSecurityClaim",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 32, nullable: false),
                    SecurityClaimId = table.Column<string>(maxLength: 32, nullable: false),
                    Value = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSecurityClaim", x => new { x.RoleId, x.SecurityClaimId });
                    table.ForeignKey(
                        name: "FK_RoleSecurityClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RoleSecurityClaim_SecurityClaim_SecurityClaimId",
                        column: x => x.SecurityClaimId,
                        principalTable: "SecurityClaim",
                        principalColumn: "SecurityClaimId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_CreatedBy",
                table: "Role",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_LastUpdatedBy",
                table: "Role",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Role_ParentRoleId",
                table: "Role",
                column: "ParentRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleSecurityClaim_SecurityClaimId",
                table: "RoleSecurityClaim",
                column: "SecurityClaimId");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaim_CreatedBy",
                table: "SecurityClaim",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SecurityClaim_LastUpdatedBy",
                table: "SecurityClaim",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedBy",
                table: "User",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedBy",
                table: "User",
                column: "LastUpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_UserProvider_UserId",
                table: "UserProvider",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserId",
                table: "UserRole",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Verification_UserId",
                table: "Verification",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleSecurityClaim");

            migrationBuilder.DropTable(
                name: "UserProvider");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Verification");

            migrationBuilder.DropTable(
                name: "SecurityClaim");

            migrationBuilder.DropTable(
                name: "Provider");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
