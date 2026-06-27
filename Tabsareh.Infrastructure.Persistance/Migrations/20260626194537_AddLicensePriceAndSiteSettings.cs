using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabsareh.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddLicensePriceAndSiteSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SettlementBasePrice",
                table: "Courses");

            migrationBuilder.AddColumn<decimal>(
                name: "LicensePriceSnapshot",
                table: "OrderItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Key);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.DropColumn(
                name: "LicensePriceSnapshot",
                table: "OrderItems");

            migrationBuilder.AddColumn<decimal>(
                name: "SettlementBasePrice",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
