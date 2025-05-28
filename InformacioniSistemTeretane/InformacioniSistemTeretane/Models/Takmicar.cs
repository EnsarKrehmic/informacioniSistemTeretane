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
        [Display(Name = "Klijent")]
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Required]
        [Display(Name = "Disciplina")]
        public int DisciplinaId { get; set; }

        [ForeignKey(nameof(DisciplinaId))]
        [ValidateNever]
        public Disciplina Disciplina { get; set; }

        [Display(Name = "Rezultat")]
        [DataType(DataType.Currency)]
        public decimal Rezultat { get; set; }

        [Display(Name = "Pozicija")]
        public int Pozicija { get; set; }
    }
}
