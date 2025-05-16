using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Klijent
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Ime { get; set; }

        [Required, MaxLength(100)]
        public string Prezime { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumRodjenja { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        [ValidateNever]
        public ApplicationUser User { get; set; }

        [ValidateNever]
        public ICollection<Uplata> Uplate { get; set; } = new List<Uplata>();

        [ValidateNever]
        public ICollection<ZakazaniGrupni> GrupneRezervacije { get; set; } = new List<ZakazaniGrupni>();

        [ValidateNever]
        public ICollection<PersonalniTrening> PersonalniTreninzi { get; set; } = new List<PersonalniTrening>();

        [ValidateNever]
        public ICollection<ProbniTrening> ProbniTreninzi { get; set; } = new List<ProbniTrening>();

        [ValidateNever]
        public ICollection<PrijavljeniGrupni> GrupnePrijave { get; set; } = new List<PrijavljeniGrupni>();

        [ValidateNever]
        public ICollection<Takmicar> Takmicenja { get; set; } = new List<Takmicar>();

        [ValidateNever]
        public ICollection<Licenca> Licence { get; set; } = new List<Licenca>();
    }
}
