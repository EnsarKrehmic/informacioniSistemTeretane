// Models/Takmicar.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Takmicar
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Required]
        public int DisciplinaId { get; set; }
        [ForeignKey(nameof(DisciplinaId))]
        [ValidateNever]
        public Disciplina Disciplina { get; set; }

        public decimal Rezultat { get; set; }
        public int Pozicija { get; set; }
    }
}
