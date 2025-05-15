using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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
        [ValidateNever]
        public Lokacija Lokacija { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        [Required]
        public decimal Kotizacija { get; set; }

        [ValidateNever]
        public ICollection<Disciplina> Discipline { get; set; } = new List<Disciplina>();
    }
}
