using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    public partial class CollectionAndDecks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Decks_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerCards",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    CardID = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "date", nullable: false),
                    OwnerID = table.Column<string>(type: "nvarchar(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCards", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlayerCards_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Users",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DeckCards",
                columns: table => new
                {
                    DeckID = table.Column<string>(type: "nvarchar(36)", nullable: false),
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Mode = table.Column<int>(type: "int", nullable: false),
                    CardID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PC = table.Column<string>(type: "nvarchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckCards", x => new { x.DeckID, x.ID });
                    table.ForeignKey(
                        name: "FK_DeckCards_Decks_DeckID",
                        column: x => x.DeckID,
                        principalTable: "Decks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckCards_PlayerCards_PC",
                        column: x => x.PC,
                        principalTable: "PlayerCards",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_PC",
                table: "DeckCards",
                column: "PC",
                unique: true,
                filter: "[PC] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_OwnerID",
                table: "Decks",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCards_OwnerID",
                table: "PlayerCards",
                column: "OwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckCards");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "PlayerCards");
        }
    }
}
