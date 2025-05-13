using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InformacioniSistemTeretane.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModelsDbContextDatabaseConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Klijent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DatumRodjenja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klijent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Klijent_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LicencniProgram",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TrajanjeDana = table.Column<int>(type: "int", nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LicencniProgram", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lokacija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Adresa = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    KontaktTelefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacija", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paket",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TrajanjeDana = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    VrstaTreninga = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trening", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zaposlenik",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Pozicija = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zaposlenik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Zaposlenik_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Licenca",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidnaDo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Licenca", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Licenca_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Licenca_LicencniProgram_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "LicencniProgram",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Igraonica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LokacijaId = table.Column<int>(type: "int", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kapacitet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Igraonica", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Igraonica_Lokacija_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sala",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LokacijaId = table.Column<int>(type: "int", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Kapacitet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sala", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sala_Lokacija_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Takmicenje",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LokacijaId = table.Column<int>(type: "int", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Kotizacija = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Takmicenje_Lokacija_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacija",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Uplata",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    PaketId = table.Column<int>(type: "int", nullable: false),
                    DatumUplate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NacinUplate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Iznos = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uplata", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Uplata_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Uplata_Paket_PaketId",
                        column: x => x.PaketId,
                        principalTable: "Paket",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Trener",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZaposlenikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trener", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trener_Zaposlenik_ZaposlenikId",
                        column: x => x.ZaposlenikId,
                        principalTable: "Zaposlenik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IgraonicaPonuda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IgraonicaId = table.Column<int>(type: "int", nullable: false),
                    OpisUsluge = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Trajanje = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IgraonicaPonuda", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IgraonicaPonuda_Igraonica_IgraonicaId",
                        column: x => x.IgraonicaId,
                        principalTable: "Igraonica",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Disciplina",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TakmicenjeId = table.Column<int>(type: "int", nullable: false),
                    Naziv = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MaxUcesnika = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disciplina", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Disciplina_Takmicenje_TakmicenjeId",
                        column: x => x.TakmicenjeId,
                        principalTable: "Takmicenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GrupniTrening",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SalaId = table.Column<int>(type: "int", nullable: false),
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vrijeme = table.Column<TimeSpan>(type: "time", nullable: false),
                    MaxUcesnika = table.Column<int>(type: "int", nullable: false)
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
                    TrenerId = table.Column<int>(type: "int", nullable: false),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Vrijeme = table.Column<TimeSpan>(type: "time", nullable: false),
                    Napredak = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Sudija",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZaposlenikId = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sudija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sudija_Disciplina_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sudija_Zaposlenik_ZaposlenikId",
                        column: x => x.ZaposlenikId,
                        principalTable: "Zaposlenik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Takmicar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    DisciplinaId = table.Column<int>(type: "int", nullable: false),
                    Rezultat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Pozicija = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Takmicar_Disciplina_DisciplinaId",
                        column: x => x.DisciplinaId,
                        principalTable: "Disciplina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Takmicar_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrijavljeniGrupni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupniTreningId = table.Column<int>(type: "int", nullable: false),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    Prisutan = table.Column<bool>(type: "bit", nullable: false),
                    VrijemeDolaska = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrijavljeniGrupni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrijavljeniGrupni_GrupniTrening_GrupniTreningId",
                        column: x => x.GrupniTreningId,
                        principalTable: "GrupniTrening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrijavljeniGrupni_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ZakazaniGrupni",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrupniTreningId = table.Column<int>(type: "int", nullable: false),
                    KlijentId = table.Column<int>(type: "int", nullable: false),
                    DatumPrijave = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZakazaniGrupni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZakazaniGrupni_GrupniTrening_GrupniTreningId",
                        column: x => x.GrupniTreningId,
                        principalTable: "GrupniTrening",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ZakazaniGrupni_Klijent_KlijentId",
                        column: x => x.KlijentId,
                        principalTable: "Klijent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Disciplina_TakmicenjeId",
                table: "Disciplina",
                column: "TakmicenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupniTrening_SalaId",
                table: "GrupniTrening",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupniTrening_TrenerId",
                table: "GrupniTrening",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Igraonica_LokacijaId",
                table: "Igraonica",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_IgraonicaPonuda_IgraonicaId",
                table: "IgraonicaPonuda",
                column: "IgraonicaId");

            migrationBuilder.CreateIndex(
                name: "IX_Klijent_UserId",
                table: "Klijent",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Licenca_KlijentId",
                table: "Licenca",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Licenca_ProgramId",
                table: "Licenca",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalniTrening_KlijentId",
                table: "PersonalniTrening",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalniTrening_TrenerId",
                table: "PersonalniTrening",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavljeniGrupni_GrupniTreningId",
                table: "PrijavljeniGrupni",
                column: "GrupniTreningId");

            migrationBuilder.CreateIndex(
                name: "IX_PrijavljeniGrupni_KlijentId",
                table: "PrijavljeniGrupni",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProbniTrening_KlijentId",
                table: "ProbniTrening",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProbniTrening_TrenerId",
                table: "ProbniTrening",
                column: "TrenerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sala_LokacijaId",
                table: "Sala",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sudija_DisciplinaId",
                table: "Sudija",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sudija_ZaposlenikId",
                table: "Sudija",
                column: "ZaposlenikId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicar_DisciplinaId",
                table: "Takmicar",
                column: "DisciplinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicar_KlijentId",
                table: "Takmicar",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenje_LokacijaId",
                table: "Takmicenje",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Trener_ZaposlenikId",
                table: "Trener",
                column: "ZaposlenikId");

            migrationBuilder.CreateIndex(
                name: "IX_Uplata_KlijentId",
                table: "Uplata",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Uplata_PaketId",
                table: "Uplata",
                column: "PaketId");

            migrationBuilder.CreateIndex(
                name: "IX_ZakazaniGrupni_GrupniTreningId",
                table: "ZakazaniGrupni",
                column: "GrupniTreningId");

            migrationBuilder.CreateIndex(
                name: "IX_ZakazaniGrupni_KlijentId",
                table: "ZakazaniGrupni",
                column: "KlijentId");

            migrationBuilder.CreateIndex(
                name: "IX_Zaposlenik_UserId",
                table: "Zaposlenik",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IgraonicaPonuda");

            migrationBuilder.DropTable(
                name: "Licenca");

            migrationBuilder.DropTable(
                name: "PersonalniTrening");

            migrationBuilder.DropTable(
                name: "PrijavljeniGrupni");

            migrationBuilder.DropTable(
                name: "ProbniTrening");

            migrationBuilder.DropTable(
                name: "Sudija");

            migrationBuilder.DropTable(
                name: "Takmicar");

            migrationBuilder.DropTable(
                name: "Uplata");

            migrationBuilder.DropTable(
                name: "ZakazaniGrupni");

            migrationBuilder.DropTable(
                name: "Igraonica");

            migrationBuilder.DropTable(
                name: "LicencniProgram");

            migrationBuilder.DropTable(
                name: "Disciplina");

            migrationBuilder.DropTable(
                name: "Paket");

            migrationBuilder.DropTable(
                name: "GrupniTrening");

            migrationBuilder.DropTable(
                name: "Klijent");

            migrationBuilder.DropTable(
                name: "Takmicenje");

            migrationBuilder.DropTable(
                name: "Sala");

            migrationBuilder.DropTable(
                name: "Trener");

            migrationBuilder.DropTable(
                name: "Trening");

            migrationBuilder.DropTable(
                name: "Lokacija");

            migrationBuilder.DropTable(
                name: "Zaposlenik");
        }
    }
}
