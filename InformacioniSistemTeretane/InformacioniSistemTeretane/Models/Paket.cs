using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Paket
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Naziv paketa")]
        public string Naziv { get; set; }

        [MaxLength(500)]
        [Display(Name = "Opis paketa")]
        public string Opis { get; set; }

        [Required]
        [Display(Name = "Cijena (KM)")]
        [DataType(DataType.Currency)]
        public decimal Cijena { get; set; }

        [Display(Name = "Trajanje (dana)")]
        public int TrajanjeDana { get; set; }

        [ValidateNever]
        public ICollection<Uplata> Uplate { get; set; } = new List<Uplata>();
    }
}
