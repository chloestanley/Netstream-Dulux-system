using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class Scan_Image : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ScanImageId",
                table: "Guards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Guards_ScanImageId",
                table: "Guards",
                column: "ScanImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guards_Scan_Images_ScanImageId",
                table: "Guards",
                column: "ScanImageId",
                principalTable: "Scan_Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guards_Scan_Images_ScanImageId",
                table: "Guards");

            migrationBuilder.DropIndex(
                name: "IX_Guards_ScanImageId",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "ScanImageId",
                table: "Guards");
        }
    }
}
