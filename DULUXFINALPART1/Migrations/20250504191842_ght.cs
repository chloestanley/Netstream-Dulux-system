using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class ght : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Carrier",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo1",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo2",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo3",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo4",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo5",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalPC",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TotalVolume",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Carrier",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo1",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo2",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo3",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo4",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo5",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "TotalPC",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "TotalVolume",
                table: "Scan_Images");
        }
    }
}
