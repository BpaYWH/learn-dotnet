using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FiveInARow.Migrations
{
    /// <inheritdoc />
    public partial class UserGameRecordrelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Player1Id = table.Column<int>(type: "integer", nullable: false),
                    Player2Id = table.Column<int>(type: "integer", nullable: false),
                    WinnerId = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameRecords_Users_Player1Id",
                        column: x => x.Player1Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRecords_Users_Player2Id",
                        column: x => x.Player2Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameRecords_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserGameRecords",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    GameRecordId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGameRecords", x => new { x.UserId, x.GameRecordId });
                    table.ForeignKey(
                        name: "FK_UserGameRecords_GameRecords_GameRecordId",
                        column: x => x.GameRecordId,
                        principalTable: "GameRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserGameRecords_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameRecords_Player1Id",
                table: "GameRecords",
                column: "Player1Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameRecords_Player2Id",
                table: "GameRecords",
                column: "Player2Id");

            migrationBuilder.CreateIndex(
                name: "IX_GameRecords_WinnerId",
                table: "GameRecords",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_UserGameRecords_GameRecordId",
                table: "UserGameRecords",
                column: "GameRecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserGameRecords");

            migrationBuilder.DropTable(
                name: "GameRecords");
        }
    }
}
