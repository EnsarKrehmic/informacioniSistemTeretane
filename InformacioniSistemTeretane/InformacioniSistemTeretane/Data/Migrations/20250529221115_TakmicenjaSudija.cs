using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformacioniSistemTeretane.Data.Migrations
{
    /// <inheritdoc />
    public partial class TakmicenjaSudija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SudijaId",
                table: "Takmicenje",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenje_SudijaId",
                table: "Takmicenje",
                column: "SudijaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Sudija_SudijaId",
                table: "Takmicenje",
                column: "SudijaId",
                principalTable: "Sudija",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Sudija_SudijaId",
                table: "Takmicenje");

            migrationBuilder.DropIndex(
                name: "IX_Takmicenje_SudijaId",
                table: "Takmicenje");

            migrationBuilder.DropColumn(
                name: "SudijaId",
                table: "Takmicenje");
        }
    }
}
