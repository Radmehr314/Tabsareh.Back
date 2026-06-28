using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabsareh.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddGatewayPaymentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GatewayRRN",
                table: "Orders",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayRefNum",
                table: "Orders",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewaySecurePan",
                table: "Orders",
                type: "nvarchar(32)",
                maxLength: 32,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayToken",
                table: "Orders",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GatewayTraceNo",
                table: "Orders",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GatewayRRN",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GatewayRefNum",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GatewaySecurePan",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GatewayToken",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "GatewayTraceNo",
                table: "Orders");
        }
    }
}
