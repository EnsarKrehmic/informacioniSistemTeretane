using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class LicencniProgram
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Naziv programa")]
        public string Naziv { get; set; }

        [MaxLength(500)]
        [Display(Name = "Opis programa")]
        public string Opis { get; set; }

        [Display(Name = "Trajanje (dana)")]
        public int TrajanjeDana { get; set; }

        [Display(Name = "Cijena (KM)")]
        [DataType(DataType.Currency)]
        public decimal Cijena { get; set; }

        [ValidateNever]
        public ICollection<Licenca> Licence { get; set; } = new List<Licenca>();
    }
}
