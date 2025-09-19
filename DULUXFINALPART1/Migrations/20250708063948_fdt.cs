using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class fdt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Guard_SecurityCard_No",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Guard_Name",
                table: "Guards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_SecurityCard_No",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Name",
                table: "Guards",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerColor",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerDiscNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerEngineNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerExpiryDate",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerMake",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerModel",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerPlateNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerRegNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerType",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrailerVin",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleColor",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleEngineNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleExpiryDate",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleLicenseDiscNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleMake",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleModel",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehiclePlateNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleRegNumber",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleType",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleVin",
                table: "Guards",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrailerColor",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerDiscNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerEngineNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerExpiryDate",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerMake",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerModel",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerPlateNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerRegNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerType",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "TrailerVin",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleColor",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleEngineNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleExpiryDate",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleLicenseDiscNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleMake",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleModel",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehiclePlateNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleRegNumber",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleType",
                table: "Guards");

            migrationBuilder.DropColumn(
                name: "VehicleVin",
                table: "Guards");

            migrationBuilder.AlterColumn<string>(
                name: "Guard_SecurityCard_No",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Guard_Name",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_SecurityCard_No",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Driver_Name",
                table: "Guards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
