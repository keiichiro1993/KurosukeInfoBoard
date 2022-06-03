using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp1.Migrations
{
    public partial class AddSynchronization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSynchronized",
                table: "CombinedControl",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.DropColumn(
                name: "IsSynchronized",
                table: "CombinedControl");
        }
    }
}
