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
        public string Naziv { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        [Required]
        public decimal Cijena { get; set; }

        public int TrajanjeDana { get; set; }

        [ValidateNever]
        public ICollection<Uplata> Uplate { get; set; } = new List<Uplata>();
    }
}
