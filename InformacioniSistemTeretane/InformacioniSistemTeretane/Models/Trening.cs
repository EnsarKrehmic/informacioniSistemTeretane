using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public abstract class Trening
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Naziv treninga")]
        public string Naziv { get; set; }

        [MaxLength(500)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Required, MaxLength(50)]
        [Display(Name = "Vrsta treninga")]
        public string VrstaTreninga { get; set; }
    }
}
