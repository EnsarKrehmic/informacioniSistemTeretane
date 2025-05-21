// Models/LicencniProgram.cs
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
        public string Naziv { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        public int TrajanjeDana { get; set; }

        public decimal Cijena { get; set; }

        [ValidateNever]
        public ICollection<Licenca> Licence { get; set; } = new List<Licenca>();
    }
}
