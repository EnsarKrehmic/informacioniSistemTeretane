using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Lokacija
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Naziv { get; set; }

        [Required, MaxLength(250)]
        public string Adresa { get; set; }

        [MaxLength(20)]
        public string KontaktTelefon { get; set; }

        [MaxLength(100), EmailAddress]
        public string Email { get; set; }

        public ICollection<Sala> Sale { get; set; }
        public ICollection<Igraonica> Igraonice { get; set; }
        public ICollection<Takmicenje> Takmicenja { get; set; }
    }
}
