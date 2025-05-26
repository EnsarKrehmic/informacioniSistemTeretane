using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace InformacioniSistemTeretane.Models
{
    public class ZakazaniGrupni
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GrupniTreningId { get; set; }
        [ForeignKey(nameof(GrupniTreningId))]
        [ValidateNever]
        public GrupniTrening GrupniTrening { get; set; }

        [Required]
        public int KlijentId { get; set; }
        [ForeignKey(nameof(KlijentId))]
        [ValidateNever]
        public Klijent Klijent { get; set; }

        [DataType(DataType.Date)]
        public DateTime DatumPrijave { get; set; }
    }
}