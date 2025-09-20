using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreDeliveryNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo10",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo11",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo12",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo13",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo14",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo15",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo16",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo17",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo18",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo19",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo20",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo21",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo22",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo23",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo24",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo25",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo6",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo7",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo8",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryNo9",
                table: "Scan_Images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryNo10",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo11",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo12",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo13",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo14",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo15",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo16",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo17",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo18",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo19",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo20",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo21",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo22",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo23",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo24",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo25",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo6",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo7",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo8",
                table: "Scan_Images");

            migrationBuilder.DropColumn(
                name: "DeliveryNo9",
                table: "Scan_Images");
        }
    }
}
