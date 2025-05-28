using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Disciplina
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Takmičenje")]
        public int TakmicenjeId { get; set; }

        [ForeignKey(nameof(TakmicenjeId))]
        [ValidateNever]
        [Display(Name = "Takmičenje")]
        public Takmicenje Takmicenje { get; set; }

        [Required, MaxLength(100)]
        [Display(Name = "Naziv discipline")]
        public string Naziv { get; set; }

        [MaxLength(500)]
        [Display(Name = "Opis discipline")]
        public string Opis { get; set; }

        [Display(Name = "Maksimalni broj učesnika")]
        public int MaxUcesnika { get; set; }

        [ValidateNever]
        public ICollection<Takmicar> Takmicari { get; set; } = new List<Takmicar>();
    }
}
