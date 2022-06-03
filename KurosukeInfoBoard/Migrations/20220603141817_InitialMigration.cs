using Microsoft.EntityFrameworkCore.Migrations;

namespace KurosukeInfoBoard.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CombinedControl",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeviceName = table.Column<string>(nullable: false),
                    RemoID = table.Column<string>(nullable: false),
                    RemoName = table.Column<string>(nullable: false),
                    HueID = table.Column<string>(nullable: false),
                    HueName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombinedControl", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CombinedControl");
        }
    }
}
