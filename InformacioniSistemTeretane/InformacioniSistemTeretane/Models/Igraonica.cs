using System.Collections.Generic;
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
        [Display(Name = "Lokacija")]
        public int LokacijaId { get; set; }

        [ForeignKey(nameof(LokacijaId))]
        [ValidateNever]
        public Lokacija Lokacija { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Naziv igraonice")]
        public string Naziv { get; set; }

        [Display(Name = "Kapacitet")]
        public int Kapacitet { get; set; }

        [ValidateNever]
        public ICollection<IgraonicaPonuda> Ponude { get; set; }
            = new List<IgraonicaPonuda>();
    }
}
