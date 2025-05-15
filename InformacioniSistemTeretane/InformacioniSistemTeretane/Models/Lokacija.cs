using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Lokacija
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Naziv { get; set; }

        [Required, MaxLength(250)]
        public string Adresa { get; set; }

        [MaxLength(20)]
        public string KontaktTelefon { get; set; }

        [MaxLength(100), EmailAddress]
        public string Email { get; set; }

        // Ove kolekcije NEĆE se validirati i inicijalno su prazne liste
        [ValidateNever]
        public ICollection<Sala> Sale { get; set; } = new List<Sala>();

        [ValidateNever]
        public ICollection<Igraonica> Igraonice { get; set; } = new List<Igraonica>();

        [ValidateNever]
        public ICollection<Takmicenje> Takmicenja { get; set; } = new List<Takmicenje>();
    }
}
