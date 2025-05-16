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
        public int TakmicenjeId { get; set; }

        [ForeignKey(nameof(TakmicenjeId))]
        [ValidateNever]
        public Takmicenje Takmicenje { get; set; }

        [Required, MaxLength(100)]
        public string Naziv { get; set; }

        [MaxLength(500)]
        public string Opis { get; set; }

        public int MaxUcesnika { get; set; }

        [ValidateNever]
        public ICollection<Takmicar> Takmicari { get; set; } = new List<Takmicar>();
    }
}
