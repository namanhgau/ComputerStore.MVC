using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ComputerStore.MVC.Migrations
{
    public partial class FixReviewTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Lệnh này sẽ XÓA bảng Reviews cũ bị thiếu cột (nếu có)
            migrationBuilder.Sql("IF OBJECT_ID('Reviews', 'U') IS NOT NULL DROP TABLE Reviews;");

            // 2. Tạo lại bảng Reviews mới tinh, đầy đủ 100% các cột
            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    AppUserUserId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reviews__74BC79CE707A5254", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_AppUsers_AppUserUserId",
                        column: x => x.AppUserUserId,
                        principalTable: "AppUsers",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK__Reviews__Product__5070F446",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            // 3. Đánh index để tăng tốc độ tìm kiếm bình luận
            migrationBuilder.CreateIndex(
                name: "IX_Reviews_AppUserUserId",
                table: "Reviews",
                column: "AppUserUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_ProductId",
                table: "Reviews",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Reviews");
        }
    }
}