using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class kjl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SignatureData",
                table: "Guards");

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SignatureData",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
