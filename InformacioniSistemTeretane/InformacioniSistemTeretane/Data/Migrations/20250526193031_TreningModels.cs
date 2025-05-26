using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformacioniSistemTeretane.Data.Migrations
{
    /// <inheritdoc />
    public partial class TreningModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrijavljeniGrupni_GrupniTrening_GrupniTreningId",
                table: "PrijavljeniGrupni");

            migrationBuilder.DropForeignKey(
                name: "FK_ZakazaniGrupni_GrupniTrening_GrupniTreningId",
                table: "ZakazaniGrupni");

            migrationBuilder.DropTable(
                name: "GrupniTrening");

            migrationBuilder.DropTable(
                name: "PersonalniTrening");

            migrationBuilder.DropTable(
                name: "ProbniTrening");

            migrationBuilder.AddColumn<DateTime>(
                name: "Datum",
                table: "Trening",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GrupniTrening_Datum",
                table: "Trening",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GrupniTrening_TrenerId",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "GrupniTrening_Vrijeme",
                table: "Trening",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KlijentId",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxUcesnika",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Napredak",
                table: "Trening",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Ocjena",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PersonalniTrening_Datum",
                table: "Trening",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonalniTrening_KlijentId",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PersonalniTrening_TrenerId",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalaId",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrenerId",
                table: "Trening",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Vrijeme",
                table: "Trening",
                type: "time",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Trening_GrupniTrening_TrenerId",
                table: "Trening",
                column: "GrupniTrening_TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_KlijentId",
                table: "Trening",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_PersonalniTrening_KlijentId",
                table: "Trening",
                column: "PersonalniTrening_KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_PersonalniTrening_TrenerId",
                table: "Trening",
                column: "PersonalniTrening_TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_SalaId",
                table: "Trening",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Trening_TrenerId",
                table: "Trening",
                column: "TrenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavljeniGrupni_Trening_GrupniTreningId",
                table: "PrijavljeniGrupni",
                column: "GrupniTreningId",
                principalTable: "Trening",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Klijent_KlijentId",
                table: "Trening",
                column: "KlijentId",
                principalTable: "Klijent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Klijent_PersonalniTrening_KlijentId",
                table: "Trening",
                column: "PersonalniTrening_KlijentId",
                principalTable: "Klijent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Sala_SalaId",
                table: "Trening",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Trener_GrupniTrening_TrenerId",
                table: "Trening",
                column: "GrupniTrening_TrenerId",
                principalTable: "Trener",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Trener_PersonalniTrening_TrenerId",
                table: "Trening",
                column: "PersonalniTrening_TrenerId",
                principalTable: "Trener",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Trening_Trener_TrenerId",
                table: "Trening",
                column: "TrenerId",
                principalTable: "Trener",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ZakazaniGrupni_Trening_GrupniTreningId",
                table: "ZakazaniGrupni",
                column: "GrupniTreningId",
                principalTable: "Trening",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrijavljeniGrupni_Trening_GrupniTreningId",
                table: "PrijavljeniGrupni");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Klijent_KlijentId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Klijent_PersonalniTrening_KlijentId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Sala_SalaId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Trener_GrupniTrening_TrenerId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Trener_PersonalniTrening_TrenerId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_Trening_Trener_TrenerId",
                table: "Trening");

            migrationBuilder.DropForeignKey(
                name: "FK_ZakazaniGrupni_Trening_GrupniTreningId",
                table: "ZakazaniGrupni");

            migrationBuilder.DropIndex(
                name: "IX_Trening_GrupniTrening_TrenerId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Trening_KlijentId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Trening_PersonalniTrening_KlijentId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Trening_PersonalniTrening_TrenerId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Trening_SalaId",
                table: "Trening");

            migrationBuilder.DropIndex(
                name: "IX_Trening_TrenerId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Datum",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "GrupniTrening_Datum",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "GrupniTrening_TrenerId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "GrupniTrening_Vrijeme",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "KlijentId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "MaxUcesnika",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Napredak",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Ocjena",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "PersonalniTrening_Datum",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "PersonalniTrening_KlijentId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "PersonalniTrening_TrenerId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "SalaId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "TrenerId",
                table: "Trening");

            migrationBuilder.DropColumn(
                name: "Vrijeme",
                table: "Trening");

            migrationBuilder.CreateTable(
                name: "GrupniTrening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SalaId = table.Column<int>(type: "int", nullable: false),
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxUcesnika = table.Column<int>(type: "int", nullable: false),
                    Vrijeme = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupniTrening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupniTrening_Sala_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Sala",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupniTrening_Trener_TrenerId",
                        column: x => x.TrenerId,
                        principalTable: "Trener",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupniTrening_Trening_Id",
                        column: x => x.Id,
                        principalTable: "Trening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalniTrening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Napredak = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Vrijeme = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalniTrening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalniTrening_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalniTrening_Trener_TrenerId",
                        column: x => x.TrenerId,
                        principalTable: "Trener",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalniTrening_Trening_Id",
                        column: x => x.Id,
                        principalTable: "Trening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProbniTrening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ocjena = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProbniTrening", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProbniTrening_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProbniTrening_Trener_TrenerId",
                        column: x => x.TrenerId,
                        principalTable: "Trener",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProbniTrening_Trening_Id",
                        column: x => x.Id,
                        principalTable: "Trening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrupniTrening_SalaId",
                table: "GrupniTrening",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupniTrening_TrenerId",
                table: "GrupniTrening",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalniTrening_KlijentId",
                table: "PersonalniTrening",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalniTrening_TrenerId",
                table: "PersonalniTrening",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProbniTrening_KlijentId",
                table: "ProbniTrening",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProbniTrening_TrenerId",
                table: "ProbniTrening",
                column: "TrenerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrijavljeniGrupni_GrupniTrening_GrupniTreningId",
                table: "PrijavljeniGrupni",
                column: "GrupniTreningId",
                principalTable: "GrupniTrening",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ZakazaniGrupni_GrupniTrening_GrupniTreningId",
                table: "ZakazaniGrupni",
                column: "GrupniTreningId",
                principalTable: "GrupniTrening",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
