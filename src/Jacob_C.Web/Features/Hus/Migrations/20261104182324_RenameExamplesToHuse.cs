using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jacob_C.Web.Features.Hus.Migrations
{
    /// <inheritdoc />
    public partial class RenameExamplesToHuse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Examples",
                table: "Examples");

            migrationBuilder.RenameTable(
                name: "Examples",
                newName: "Huse");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Huse",
                table: "Huse",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Huse",
                table: "Huse");

            migrationBuilder.RenameTable(
                name: "Huse",
                newName: "Examples");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Examples",
                table: "Examples",
                column: "Id");
        }
    }
}
