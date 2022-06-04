using Microsoft.EntityFrameworkCore.Migrations;

namespace DBMigrationDummy.Migrations
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
                    RemoID = table.Column<string>(nullable: true),
                    RemoName = table.Column<string>(nullable: true),
                    HueID = table.Column<string>(nullable: true),
                    HueName = table.Column<string>(nullable: true),
                    IsSynchronized = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CombinedControl", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SynchronizedRemoItem",
                columns: table => new
                {
                    ID = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CombinedControlID = table.Column<long>(nullable: false),
                    ApplianceId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SynchronizedRemoItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SynchronizedRemoItem_CombinedControl_CombinedControlID",
                        column: x => x.CombinedControlID,
                        principalTable: "CombinedControl",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SynchronizedRemoItem_CombinedControlID",
                table: "SynchronizedRemoItem",
                column: "CombinedControlID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SynchronizedRemoItem");

            migrationBuilder.DropTable(
                name: "CombinedControl");
        }
    }
}
