using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Sudija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Zaposlenik")]
        public int ZaposlenikId { get; set; }

        [ForeignKey(nameof(ZaposlenikId))]
        [ValidateNever]
        public Zaposlenik Zaposlenik { get; set; }

        [Required]
        [Display(Name = "Disciplina")]
        public int DisciplinaId { get; set; }

        [ForeignKey(nameof(DisciplinaId))]
        [ValidateNever]
        public Disciplina Disciplina { get; set; }
    }
}
