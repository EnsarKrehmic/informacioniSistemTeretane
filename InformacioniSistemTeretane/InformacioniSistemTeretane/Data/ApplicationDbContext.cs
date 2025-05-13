using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InformacioniSistemTeretane.Models;

namespace InformacioniSistemTeretane.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Disciplina> Disciplina { get; set; }
        public DbSet<GrupniTrening> GrupniTrening { get; set; }
        public DbSet<Igraonica> Igraonica { get; set; }
        public DbSet<IgraonicaPonuda> IgraonicaPonuda { get; set; }
        public DbSet<Klijent> Klijent { get; set; }
        public DbSet<Licenca> Licenca { get; set; }
        public DbSet<LicencniProgram> LicencniProgram { get; set; }
        public DbSet<Lokacija> Lokacija { get; set; }
        public DbSet<Paket> Paket { get; set; }
        public DbSet<PersonalniTrening> PersonalniTrening { get; set; }
        public DbSet<PrijavljeniGrupni> PrijavljeniGrupni { get; set; }
        public DbSet<ProbniTrening> ProbniTrening { get; set; }
        public DbSet<Sala> Sala { get; set; }
        public DbSet<Sudija> Sudija { get; set; }
        public DbSet<Takmicar> Takmicar { get; set; }
        public DbSet<Takmicenje> Takmicenje { get; set; }
        public DbSet<Trener> Trener { get; set; }
        public DbSet<Trening> Trening { get; set; }
        public DbSet<Uplata> Uplata { get; set; }
        public DbSet<ZakazaniGrupni> ZakazaniGrupni { get; set; }
        public DbSet<Zaposlenik> Zaposlenik { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Disciplina>().ToTable("Disciplina");
            modelBuilder.Entity<GrupniTrening>().ToTable("GrupniTrening");
            modelBuilder.Entity<Igraonica>().ToTable("Igraonica");
            modelBuilder.Entity<IgraonicaPonuda>().ToTable("IgraonicaPonuda");
            modelBuilder.Entity<Klijent>().ToTable("Klijent");
            modelBuilder.Entity<Licenca>().ToTable("Licenca");
            modelBuilder.Entity<LicencniProgram>().ToTable("LicencniProgram");
            modelBuilder.Entity<Lokacija>().ToTable("Lokacija");
            modelBuilder.Entity<Paket>().ToTable("Paket");
            modelBuilder.Entity<PersonalniTrening>().ToTable("PersonalniTrening");
            modelBuilder.Entity<PrijavljeniGrupni>().ToTable("PrijavljeniGrupni");
            modelBuilder.Entity<ProbniTrening>().ToTable("ProbniTrening");
            modelBuilder.Entity<Sala>().ToTable("Sala");
            modelBuilder.Entity<Sudija>().ToTable("Sudija");
            modelBuilder.Entity<Takmicar>().ToTable("Takmicar");
            modelBuilder.Entity<Takmicenje>().ToTable("Takmicenje");
            modelBuilder.Entity<Trener>().ToTable("Trener");
            modelBuilder.Entity<Trening>().ToTable("Trening");
            modelBuilder.Entity<Uplata>().ToTable("Uplata");
            modelBuilder.Entity<ZakazaniGrupni>().ToTable("ZakazaniGrupni");
            modelBuilder.Entity<Zaposlenik>().ToTable("Zaposlenik");

            // Konfiguracija naslijeđivanja treninga
            modelBuilder.Entity<GrupniTrening>().HasBaseType<Trening>();
            modelBuilder.Entity<PersonalniTrening>().HasBaseType<Trening>();
            modelBuilder.Entity<ProbniTrening>().HasBaseType<Trening>();

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
        }
    }
}
