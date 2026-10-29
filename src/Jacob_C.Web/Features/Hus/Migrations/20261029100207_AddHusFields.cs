using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jacob_C.Web.Features.Hus.Migrations
{
    /// <inheritdoc />
    public partial class AddHusFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Examples");

            migrationBuilder.AddColumn<int>(
                name: "Damer",
                table: "Examples",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Kvm",
                table: "Examples",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Damer",
                table: "Examples");

            migrationBuilder.DropColumn(
                name: "Kvm",
                table: "Examples");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Examples",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
