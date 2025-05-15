using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Igraonica
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LokacijaId { get; set; }

        [ForeignKey(nameof(LokacijaId))]
        [ValidateNever]
        public Lokacija Lokacija { get; set; }

        [Required, MaxLength(100)]
        public string Naziv { get; set; }

        public int Kapacitet { get; set; }

        [ValidateNever]
        public ICollection<IgraonicaPonuda> Ponude { get; set; } = new List<IgraonicaPonuda>();
    }
}
