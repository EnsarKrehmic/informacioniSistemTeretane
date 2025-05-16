using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class Trener
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ZaposlenikId { get; set; }

        [ForeignKey(nameof(ZaposlenikId))]
        [ValidateNever]
        public Zaposlenik Zaposlenik { get; set; }

        [ValidateNever]
        public ICollection<GrupniTrening> GrupniTreninzi { get; set; } = new List<GrupniTrening>();

        [ValidateNever]
        public ICollection<PersonalniTrening> PersonalniTreninzi { get; set; } = new List<PersonalniTrening>();

        [ValidateNever]
        public ICollection<ProbniTrening> ProbniTreninzi { get; set; } = new List<ProbniTrening>();
    }
}
