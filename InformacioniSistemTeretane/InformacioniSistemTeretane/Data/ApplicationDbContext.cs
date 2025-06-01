using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Disciplina> Discipline { get; set; }
        public DbSet<GrupniTrening> GrupniTreninzi { get; set; }
        public DbSet<Igraonica> Igraonice { get; set; }
        public DbSet<IgraonicaPonuda> IgraonicaPonude { get; set; }
        public DbSet<Klijent> Klijenti { get; set; }
        public DbSet<Licenca> Licence { get; set; }
        public DbSet<LicencniProgram> LicencniProgrami { get; set; }
        public DbSet<Lokacija> Lokacije { get; set; }
        public DbSet<Mapa> Mape { get; set; }
        public DbSet<Paket> Paketi { get; set; }
        public DbSet<PersonalniTrening> PersonalniTreninzi { get; set; }
        public DbSet<PrijavljeniGrupni> PrijavljeniGrupni { get; set; }
        public DbSet<ProbniTrening> ProbniTreninzi { get; set; }
        public DbSet<Sala> Sale { get; set; }
        public DbSet<Sudija> Sudije { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
        public DbSet<Takmicar> Takmicari { get; set; }
        public DbSet<Takmicenje> Takmicenja { get; set; }
        public DbSet<Trener> Treneri { get; set; }
        public DbSet<Trening> Treninzi { get; set; }
        public DbSet<Uplata> Uplate { get; set; }
        public DbSet<VideoSadrzaj> VideoSadrzaji { get; set; }
        public DbSet<ZakazaniGrupni> ZakazaniGrupni { get; set; }
        public DbSet<Zaposlenik> Zaposlenici { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Disciplina>().ToTable("Disciplina");
            modelBuilder.Entity<Igraonica>().ToTable("Igraonica");
            modelBuilder.Entity<IgraonicaPonuda>().ToTable("IgraonicaPonuda");
            modelBuilder.Entity<Klijent>().ToTable("Klijent");
            modelBuilder.Entity<Licenca>().ToTable("Licenca");
            modelBuilder.Entity<LicencniProgram>().ToTable("LicencniProgram");
            modelBuilder.Entity<Lokacija>().ToTable("Lokacija");
            modelBuilder.Entity<Paket>().ToTable("Paket");
            modelBuilder.Entity<PrijavljeniGrupni>().ToTable("PrijavljeniGrupni");
            modelBuilder.Entity<Sala>().ToTable("Sala");
            modelBuilder.Entity<Sudija>().ToTable("Sudija");
            modelBuilder.Entity<Takmicar>().ToTable("Takmicar");
            modelBuilder.Entity<Takmicenje>().ToTable("Takmicenje");
            modelBuilder.Entity<Trener>().ToTable("Trener");
            modelBuilder.Entity<Trening>().ToTable("Trening");
            modelBuilder.Entity<Uplata>().ToTable("Uplata");
            modelBuilder.Entity<ZakazaniGrupni>().ToTable("ZakazaniGrupni");
            modelBuilder.Entity<Zaposlenik>().ToTable("Zaposlenik");

            // Konfiguracija za Screenshot
            modelBuilder.Entity<Screenshot>()
                .Ignore(s => s.DatotekaSlike);

            modelBuilder.Entity<Screenshot>()
                .Property(s => s.DatumKreiranja)
                .HasDefaultValueSql("GETDATE()");

            // Konfiguracija za VideoSadrzaj
            modelBuilder.Entity<VideoSadrzaj>()
                .Property(v => v.DatumDodavanja)
                .HasDefaultValueSql("GETDATE()");

            // Konfiguracija za Mapu
            modelBuilder.Entity<Mapa>()
                .Property(l => l.Tip)
                .HasDefaultValue(TipMape.Ostalo);

            // TPH mapiranje za sve treninge u jednoj tablici "Treninzi"
            modelBuilder.Entity<Trening>()
                .HasDiscriminator<string>("VrstaTreninga")
                .HasValue<GrupniTrening>("Grupni")
                .HasValue<PersonalniTrening>("Personalni")
                .HasValue<ProbniTrening>("Probni");

            modelBuilder.Entity<Uplata>()
                .Property(u => u.Iznos)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Paket>()
                .Property(p => p.Cijena)
                .HasPrecision(18, 2);

            modelBuilder.Entity<IgraonicaPonuda>()
                .Property(i => i.Cijena)
                .HasPrecision(18, 2);

            modelBuilder.Entity<LicencniProgram>()
                .Property(l => l.Cijena)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Takmicenje>()
                .Property(t => t.Kotizacija)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Takmicar>()
                .Property(t => t.Rezultat)
                .HasPrecision(18, 2);

            modelBuilder.Entity<PersonalniTrening>()
                .HasOne(pt => pt.Trener)
                .WithMany(t => t.PersonalniTreninzi)
                .HasForeignKey(pt => pt.TrenerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PersonalniTrening>()
                .HasOne(pt => pt.Klijent)
                .WithMany(k => k.PersonalniTreninzi)
                .HasForeignKey(pt => pt.KlijentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProbniTrening>()
                .HasOne(pt => pt.Trener)
                .WithMany(t => t.ProbniTreninzi)
                .HasForeignKey(pt => pt.TrenerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProbniTrening>()
                .HasOne(pt => pt.Klijent)
                .WithMany(k => k.ProbniTreninzi)
                .HasForeignKey(pt => pt.KlijentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PrijavljeniGrupni>()
                .HasOne(pg => pg.Klijent)
                .WithMany(k => k.GrupnePrijave)
                .HasForeignKey(pg => pg.KlijentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ZakazaniGrupni>()
                .HasOne(zg => zg.Klijent)
                .WithMany(k => k.GrupneRezervacije)
                .HasForeignKey(zg => zg.KlijentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Takmicenje>()
                .HasOne(t => t.Sudija)
                .WithMany()
                .HasForeignKey(t => t.SudijaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
