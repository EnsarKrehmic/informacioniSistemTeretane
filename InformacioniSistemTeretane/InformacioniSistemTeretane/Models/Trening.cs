using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public abstract class Trening
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Naziv { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        [Required, MaxLength(50)]
        public string VrstaTreninga { get; set; }
    }
}
