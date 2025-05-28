using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Lokacija
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "Naziv lokacije")]
        public string Naziv { get; set; }

        [Required, MaxLength(250)]
        [Display(Name = "Adresa")]
        public string Adresa { get; set; }

        [MaxLength(20)]
        [Display(Name = "Kontakt telefon")]
        public string KontaktTelefon { get; set; }

        [MaxLength(100), EmailAddress]
        [Display(Name = "Email adresa")]
        public string Email { get; set; }

        [ValidateNever]
        public ICollection<Sala> Sale { get; set; } = new List<Sala>();

        [ValidateNever]
        public ICollection<Igraonica> Igraonice { get; set; } = new List<Igraonica>();

        [ValidateNever]
        public ICollection<Takmicenje> Takmicenja { get; set; } = new List<Takmicenje>();
    }
}
