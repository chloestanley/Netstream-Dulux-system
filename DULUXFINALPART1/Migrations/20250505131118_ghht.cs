using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DULUXFINALPART1.Migrations
{
    /// <inheritdoc />
    public partial class ghht : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "Guards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "Guards");
        }
    }
}
