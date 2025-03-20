using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BE_S6_L1.Migrations
{
    /// <inheritdoc />
    public partial class AddRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Valutazione",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudenteId = table.Column<int>(type: "int", nullable: false),
                    Materia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Voto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataValutazione = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Valutazione", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Valutazione_Studenti_StudenteId",
                        column: x => x.StudenteId,
                        principalTable: "Studenti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Valutazione_StudenteId",
                table: "Valutazione",
                column: "StudenteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Valutazione");
        }
    }
}
