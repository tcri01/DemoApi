using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberToContactSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Notes",
                table: "Members",
                newName: "Mobile");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "Members",
                newName: "Content");

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "Members",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Company",
                table: "Members");

            migrationBuilder.RenameColumn(
                name: "Mobile",
                table: "Members",
                newName: "Notes");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Members",
                newName: "Bio");
        }
    }
}
