using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Sala
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int LokacijaId { get; set; }

        [ForeignKey(nameof(LokacijaId))]
        [ValidateNever] // onemogućava validaciju navigacijskog svojstva
        public Lokacija Lokacija { get; set; }

        [Required, MaxLength(100)]
        public string Naziv { get; set; }

        public int Kapacitet { get; set; }
    }
}
