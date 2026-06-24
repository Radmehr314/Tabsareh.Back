using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabsareh.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class blog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    TitleImage = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Excerpt = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    CategoryId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_CategoryId",
                table: "Blogs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_IsPublished",
                table: "Blogs",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_Slug",
                table: "Blogs",
                column: "Slug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
