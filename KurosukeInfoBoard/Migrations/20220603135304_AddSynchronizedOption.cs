using Microsoft.EntityFrameworkCore.Migrations;

namespace KurosukeInfoBoard.Migrations
{
    public partial class AddSynchronizedOption : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSynchronized",
                table: "CombinedControl",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSynchronized",
                table: "CombinedControl");
        }
    }
}
