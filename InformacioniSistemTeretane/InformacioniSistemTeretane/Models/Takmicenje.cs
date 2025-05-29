using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Takmicenje
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        [Display(Name = "Naziv takmičenja")]
        public string Naziv { get; set; }

        [Display(Name = "Datum održavanja")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required]
        [Display(Name = "Lokacija")]
        public int LokacijaId { get; set; }

        [ForeignKey(nameof(LokacijaId))]
        [ValidateNever]
        public Lokacija Lokacija { get; set; }

        [Required]
        [Display(Name = "Sudija")]
        public int SudijaId { get; set; }

        [ForeignKey(nameof(SudijaId))]
        [ValidateNever]
        public Sudija Sudija { get; set; }

        [MaxLength(500)]
        [Display(Name = "Opis")]
        public string Opis { get; set; }

        [Required]
        [Display(Name = "Kotizacija (KM)")]
        [DataType(DataType.Currency)]
        public decimal Kotizacija { get; set; }

        [ValidateNever]
        public ICollection<Disciplina> Discipline { get; set; } = new List<Disciplina>();
    }
}
