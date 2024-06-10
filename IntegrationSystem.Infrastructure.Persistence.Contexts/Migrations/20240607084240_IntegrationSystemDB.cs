using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationSystem.Infrastructure.Persistence.Contexts.Migrations
{
    /// <inheritdoc />
    public partial class IntegrationSystemDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MergedUsers",
                columns: table => new
                {
                    MergedUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AzureUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    XmlId = table.Column<int>(type: "int", nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    UserPrincipalName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AzureJobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    XmlJobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AzureEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    XmlEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AzurePhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    XmlPhoneNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OfficeLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PreferredLanguage = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MergedUsers", x => x.MergedUserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MergedUsers");
        }
    }
}
