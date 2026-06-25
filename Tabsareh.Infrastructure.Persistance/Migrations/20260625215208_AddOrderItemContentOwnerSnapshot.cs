using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabsareh.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemContentOwnerSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentOwnerIdSnapshot",
                table: "OrderItems",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentOwnerNameSnapshot",
                table: "OrderItems",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ContentOwnerIdSnapshot",
                table: "OrderItems",
                column: "ContentOwnerIdSnapshot");

            migrationBuilder.Sql(@"
UPDATE oi
SET oi.ContentOwnerIdSnapshot = c.ContentOwnerId,
    oi.ContentOwnerNameSnapshot = COALESCE(co.Name, '')
FROM OrderItems oi
JOIN Courses c ON c.Id = oi.CourseId
LEFT JOIN ContentOwners co ON co.Id = c.ContentOwnerId;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ContentOwnerIdSnapshot",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ContentOwnerIdSnapshot",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ContentOwnerNameSnapshot",
                table: "OrderItems");
        }
    }
}
