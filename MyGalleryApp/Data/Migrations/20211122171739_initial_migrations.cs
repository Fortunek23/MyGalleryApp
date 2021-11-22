using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyGalleryApp.Data.Migrations
{
    public partial class initial_migrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "sharedWithMe",
                columns: table => new
                {
                    ShareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sharedWithMe", x => x.ShareId);
                });

            migrationBuilder.CreateTable(
                name: "Shared",
                columns: table => new
                {
                    ShareId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shared", x => x.ShareId);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    PhotoId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Location = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Tag = table.Column<string>(nullable: true),
                    CaptureBy = table.Column<string>(nullable: true),
                    CaptureDate = table.Column<DateTime>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    ShareId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.PhotoId);
                    table.ForeignKey(
                        name: "FK_Photo_Shared_ShareId",
                        column: x => x.ShareId,
                        principalTable: "Shared",
                        principalColumn: "ShareId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Photo_ShareId",
                table: "Photo",
                column: "ShareId");

            migrationBuilder.CreateIndex(
                name: "IX_Shared_PhotoId",
                table: "Shared",
                column: "PhotoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shared_Photo_PhotoId",
                table: "Shared",
                column: "PhotoId",
                principalTable: "Photo",
                principalColumn: "PhotoId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photo_Shared_ShareId",
                table: "Photo");

            migrationBuilder.DropTable(
                name: "sharedWithMe");

            migrationBuilder.DropTable(
                name: "Shared");

            migrationBuilder.DropTable(
                name: "Photo");
        }
    }
}
