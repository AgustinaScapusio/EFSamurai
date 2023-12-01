using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFSamurai.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class CreatedBattleLogAndBattleEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BattleLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BattleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleLogs_Battle_BattleId",
                        column: x => x.BattleId,
                        principalTable: "Battle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BattleEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Sumary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BattleLogID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BattleEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BattleEvents_BattleLogs_BattleLogID",
                        column: x => x.BattleLogID,
                        principalTable: "BattleLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BattleEvents_BattleLogID",
                table: "BattleEvents",
                column: "BattleLogID");

            migrationBuilder.CreateIndex(
                name: "IX_BattleLogs_BattleId",
                table: "BattleLogs",
                column: "BattleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BattleEvents");

            migrationBuilder.DropTable(
                name: "BattleLogs");
        }
    }
}
