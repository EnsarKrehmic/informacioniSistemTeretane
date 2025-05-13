using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InformacioniSistemTeretane.Models
{
    public class Takmicenje
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Naziv { get; set; }

        public DateTime Datum { get; set; }

        [Required]
        public int LokacijaId { get; set; }
        [ForeignKey(nameof(LokacijaId))]
        public Lokacija Lokacija { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        [Required]
        public decimal Kotizacija { get; set; }

        public ICollection<Disciplina> Discipline { get; set; }
    }
}
