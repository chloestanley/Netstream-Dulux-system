using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class ImageUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath4",
                table: "Guards",
                newName: "ImageUrl4");

            migrationBuilder.RenameColumn(
                name: "ImagePath3",
                table: "Guards",
                newName: "ImageUrl3");

            migrationBuilder.RenameColumn(
                name: "ImagePath2",
                table: "Guards",
                newName: "ImageUrl2");

            migrationBuilder.RenameColumn(
                name: "ImagePath1",
                table: "Guards",
                newName: "ImageUrl1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl4",
                table: "Guards",
                newName: "ImagePath4");

            migrationBuilder.RenameColumn(
                name: "ImageUrl3",
                table: "Guards",
                newName: "ImagePath3");

            migrationBuilder.RenameColumn(
                name: "ImageUrl2",
                table: "Guards",
                newName: "ImagePath2");

            migrationBuilder.RenameColumn(
                name: "ImageUrl1",
                table: "Guards",
                newName: "ImagePath1");
        }
    }
}
