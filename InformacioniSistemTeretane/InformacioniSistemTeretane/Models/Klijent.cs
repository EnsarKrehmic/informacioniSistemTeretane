using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace InformacioniSistemTeretane.Models
{
    public class Klijent
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Ime { get; set; }

        [Required, MaxLength(100)]
        public string Prezime { get; set; }

        public DateTime DatumRodjenja { get; set; }

        [Required]
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public ICollection<Uplata> Uplate { get; set; }
        public ICollection<ZakazaniGrupni> GrupneRezervacije { get; set; }
        public ICollection<PersonalniTrening> PersonalniTreninzi { get; set; }
        public ICollection<ProbniTrening> ProbniTreninzi { get; set; }
        public ICollection<PrijavljeniGrupni> GrupnePrijave { get; set; }
        public ICollection<Takmicar> Takmicenja { get; set; }
        public ICollection<Licenca> Licence { get; set; }
    }
}
