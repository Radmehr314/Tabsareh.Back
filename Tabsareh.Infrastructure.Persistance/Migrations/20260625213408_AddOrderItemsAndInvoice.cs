using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabsareh.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderItemsAndInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Orders",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<decimal>(
                name: "CourseDiscountAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DiscountCode",
                table: "Orders",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountCodeAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountCodePercentSnapshot",
                table: "Orders",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PayableAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "SubtotalAmount",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    OrderId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CourseTitleSnapshot = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    CoursePriceSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CourseDiscountPercentSnapshot = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    CourseDiscountAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountCodePercentSnapshot = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    DiscountCodeAmountSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FinalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SettlementBasePriceSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ContentOwnerSharePercentSnapshot = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    LicenseCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CourseId",
                table: "OrderItems",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.Sql(@"
UPDATE Orders
SET SubtotalAmount = CoursePrice,
    CourseDiscountAmount = CASE WHEN CoursePrice > Amount THEN CoursePrice - Amount ELSE 0 END,
    PayableAmount = Amount
WHERE CourseId IS NOT NULL;

INSERT INTO OrderItems (
    Id,
    OrderId,
    CourseId,
    CourseTitleSnapshot,
    CoursePriceSnapshot,
    CourseDiscountPercentSnapshot,
    CourseDiscountAmountSnapshot,
    DiscountCodePercentSnapshot,
    DiscountCodeAmountSnapshot,
    FinalAmount,
    SettlementBasePriceSnapshot,
    ContentOwnerSharePercentSnapshot,
    LicenseCode,
    CreatedAt,
    UpdatedAt)
SELECT
    REPLACE(CONVERT(nvarchar(36), NEWID()), '-', ''),
    o.Id,
    o.CourseId,
    c.Title,
    o.CoursePrice,
    o.DiscountPercentSnapshot,
    CASE WHEN o.CoursePrice > o.Amount THEN o.CoursePrice - o.Amount ELSE 0 END,
    0,
    0,
    o.Amount,
    o.SettlementBasePriceSnapshot,
    o.ContentOwnerSharePercentSnapshot,
    o.LicenseCode,
    o.CreatedAt,
    o.UpdatedAt
FROM Orders o
JOIN Courses c ON c.Id = o.CourseId
WHERE o.CourseId IS NOT NULL
  AND NOT EXISTS (SELECT 1 FROM OrderItems oi WHERE oi.OrderId = o.Id);");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CourseDiscountAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountCode",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountCodeAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountCodePercentSnapshot",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PayableAmount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SubtotalAmount",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "CourseId",
                table: "Orders",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(64)",
                oldMaxLength: 64,
                oldNullable: true);
        }
    }
}
