using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class PrijavljeniGrupni
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Grupni trening")]
        public int GrupniTreningId { get; set; }

        [ForeignKey(nameof(GrupniTreningId))]
        [ValidateNever]
        public GrupniTrening GrupniTrening { get; set; }

        [Required]
        [Display(Name = "Klijent")]
        public int KlijentId { get; set; }

        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [Display(Name = "Prisutnost")]
        public bool Prisutan { get; set; }

        [Display(Name = "Vrijeme dolaska")]
        [DataType(DataType.Time)]
        public DateTime VrijemeDolaska { get; set; }
    }
}
