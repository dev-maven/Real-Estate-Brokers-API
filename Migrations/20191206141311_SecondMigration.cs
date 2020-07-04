using Microsoft.EntityFrameworkCore.Migrations;

namespace Urban.ng.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Area",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "Baths",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "Rooms",
                table: "Plans",
                newName: "Decription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Decription",
                table: "Plans",
                newName: "Rooms");

            migrationBuilder.AddColumn<string>(
                name: "Area",
                table: "Plans",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Baths",
                table: "Plans",
                nullable: true);
        }
    }
}
