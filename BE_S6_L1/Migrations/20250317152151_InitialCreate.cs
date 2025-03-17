using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BE_S6_L1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Studenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Cognome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataDiNascita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Studenti",
                columns: new[] { "Id", "Cognome", "DataDiNascita", "Email", "Nome" },
                values: new object[,]
                {
                    { 1, "Rossi", new DateTime(1995, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "mario.rossi@example.com", "Mario" },
                    { 2, "Bianchi", new DateTime(1998, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "laura.bianchi@example.com", "Laura" },
                    { 3, "Verdi", new DateTime(1997, 3, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "giuseppe.verdi@example.com", "Giuseppe" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Studenti");
        }
    }
}
