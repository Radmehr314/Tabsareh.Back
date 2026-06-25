using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tabsareh.Infrastructure.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseSettlementPriceAndChapterVideos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SettlementBasePrice",
                table: "Courses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql("UPDATE [Courses] SET [SettlementBasePrice] = [Price] WHERE [SettlementBasePrice] = 0");

            migrationBuilder.CreateTable(
                name: "CourseChapterVideos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CourseChapterId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    ExternalVideoId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    UploadStatus = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseChapterVideos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseChapterVideos_CourseChapters_CourseChapterId",
                        column: x => x.CourseChapterId,
                        principalTable: "CourseChapters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseChapterVideos_CourseChapterId",
                table: "CourseChapterVideos",
                column: "CourseChapterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseChapterVideos");

            migrationBuilder.DropColumn(
                name: "SettlementBasePrice",
                table: "Courses");
        }
    }
}
